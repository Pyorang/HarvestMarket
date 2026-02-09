using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HybridCurrencyRepository : ICurrencyRepository
{
    private readonly PlayerPrefsCurrencyRepository _local;
#if !UNITY_WEBGL || UNITY_EDITOR
    private readonly FirebaseCurrencyRepository _remote;
#endif
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
#if !UNITY_WEBGL || UNITY_EDITOR
        _remote = new FirebaseCurrencyRepository();
        Debug.Log("[HybridCurrencyRepository] Firebase 활성화");
#else
        Debug.Log("[HybridCurrencyRepository] WebGL 모드 - 로컬 저장소만 사용");
#endif
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
#if !UNITY_WEBGL || UNITY_EDITOR
            DebouncedFirebaseSave();
#else
            Debug.Log("[HybridCurrencyRepository] WebGL 모드: 로컬 저장만 수행");
#endif
        }

        return default;
    }

#if !UNITY_WEBGL || UNITY_EDITOR
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
#endif

    public async UniTask<CurrencyData> Load()
    {
        var localData = await _local.Load();
        
#if !UNITY_WEBGL || UNITY_EDITOR
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
#else
        Debug.Log("[HybridCurrencyRepository] WebGL 모드: 로컬 데이터만 반환");
        return localData;
#endif
    }

    public UniTaskVoid Delete()
    {
        _local.Delete();
#if !UNITY_WEBGL || UNITY_EDITOR
        _remote.Delete().Forget();
#endif
        return default;
    }

    public void FlushToRemote()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
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
#else
        Debug.Log("[HybridCurrencyRepository] WebGL 모드: FlushToRemote 스킵");
#endif
    }
}
