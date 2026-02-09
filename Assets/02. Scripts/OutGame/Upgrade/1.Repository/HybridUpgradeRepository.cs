using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HybridUpgradeRepository : IUpgradeRepository
{
    private readonly PlayerPrefsUpgradeRepository _local;
#if !UNITY_WEBGL || UNITY_EDITOR
    private readonly FirebaseUpgradeRepository _remote;
#endif
    private readonly MonoBehaviour _coroutineRunner;

    private Coroutine _debounceCoroutine;
    private PlayerUpgradeData _pendingData;
    private int _localSaveCount = 0;

    private const int LOCAL_SAVE_THRESHOLD = 5;
    private const float DEBOUNCE_SECONDS = 0.6f;

    public HybridUpgradeRepository(MonoBehaviour coroutineRunner, string userKey = "")
    {
        _coroutineRunner = coroutineRunner;
        _local = new PlayerPrefsUpgradeRepository(userKey);
#if !UNITY_WEBGL || UNITY_EDITOR
        _remote = new FirebaseUpgradeRepository();
        Debug.Log("[HybridUpgradeRepository] Firebase 활성화");
#else
        Debug.Log("[HybridUpgradeRepository] WebGL 모드 - 로컬 저장소만 사용");
#endif
    }

    public UniTaskVoid Save(PlayerUpgradeData data)
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
            Debug.Log("[HybridUpgradeRepository] WebGL 모드: 로컬 저장만 수행");
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
            Debug.Log("[HybridUpgradeRepo] Firebase 저장 완료 (디바운스)");
        }

        _debounceCoroutine = null;
    }
#endif

    public async UniTask<PlayerUpgradeData> Load()
    {
        var localData = await _local.Load();

#if !UNITY_WEBGL || UNITY_EDITOR
        PlayerUpgradeData remoteData;

        try
        {
            remoteData = await _remote.Load();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[HybridUpgradeRepo] Firebase 로드 실패, 로컬 사용: {e.Message}");
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
        Debug.Log("[HybridUpgradeRepository] WebGL 모드: 로컬 데이터만 반환");
        return localData;
#endif
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
            Debug.Log("[HybridUpgradeRepo] FlushToRemote 완료");
        }
#else
        Debug.Log("[HybridUpgradeRepository] WebGL 모드: FlushToRemote 스킵");
#endif
    }
}
