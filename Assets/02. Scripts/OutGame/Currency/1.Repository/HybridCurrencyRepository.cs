using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HybridCurrencyRepository : ICurrencyRepository
{
    private readonly PlayerPrefsCurrencyRepository _local;
    private readonly FirebaseCurrencyRepository _remote;
    private readonly MonoBehaviour _coroutineRunner;

    private Coroutine _debounceCoroutine;
    private CurrencyData _pendingData;
    private int _localSaveCount = 0;

    private const int LOCAL_SAVE_THRESHOLD = 5;
    private const float DEBOUNCE_SECONDS = 0.6f;

    public HybridCurrencyRepository(MonoBehaviour coroutineRunner, string userKey = "")
    {
        _coroutineRunner = coroutineRunner;
        _local = new PlayerPrefsCurrencyRepository(userKey);
        _remote = new FirebaseCurrencyRepository();
    }

    public UniTaskVoid Save(CurrencyData data)
    {
        data.LastSavedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        _local.Save(data);
        _localSaveCount++;
        _pendingData = data;

        if (_localSaveCount >= LOCAL_SAVE_THRESHOLD)
        {
            _localSaveCount = 0;
            DebouncedFirebaseSave();
        }

        return default;
    }

    private void DebouncedFirebaseSave()
    {
        if (_debounceCoroutine != null)
            _coroutineRunner.StopCoroutine(_debounceCoroutine);

        _debounceCoroutine = _coroutineRunner.StartCoroutine(FirebaseSaveAfterDelay());
    }

    private IEnumerator FirebaseSaveAfterDelay()
    {
        yield return new WaitForSeconds(DEBOUNCE_SECONDS);

        if (_pendingData != null)
        {
            _remote.Save(_pendingData).Forget();
            Debug.Log("[HybridCurrencyRepo] Firebase 저장 완료 (디바운스)");
        }

        _debounceCoroutine = null;
    }

    public async UniTask<CurrencyData> Load()
    {
        var localData = await _local.Load();
        CurrencyData remoteData;

        try
        {
            remoteData = await _remote.Load();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[HybridCurrencyRepo] Firebase 로드 실패, 로컬 사용: {e.Message}");
            return localData;
        }

        if (localData.LastSavedAt >= remoteData.LastSavedAt)
        {
            if (localData.LastSavedAt > remoteData.LastSavedAt)
                _remote.Save(localData).Forget();
            return localData;
        }
        else
        {
            _local.Save(remoteData);
            return remoteData;
        }
    }

    public UniTaskVoid Delete()
    {
        _local.Delete();
        _remote.Delete().Forget();
        return default;
    }

    public void FlushToRemote()
    {
        if (_pendingData != null && _localSaveCount > 0)
        {
            if (_debounceCoroutine != null)
            {
                _coroutineRunner.StopCoroutine(_debounceCoroutine);
                _debounceCoroutine = null;
            }

            _remote.Save(_pendingData).Forget();
            _localSaveCount = 0;
            _pendingData = null;
            Debug.Log("[HybridCurrencyRepo] FlushToRemote 완료");
        }
    }
}
