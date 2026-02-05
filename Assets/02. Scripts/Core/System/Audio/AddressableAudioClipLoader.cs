using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableAudioClipLoader : IAudioClipLoader
{
    private readonly Dictionary<string, AudioClip> _clipCache = new();
    private readonly Dictionary<string, AsyncOperationHandle<AudioClip>> _loadHandles = new();
    private readonly string _addressPrefix;

    public AddressableAudioClipLoader(string addressPrefix = "Audio/")
    {
        _addressPrefix = addressPrefix;
    }

    public bool TryGetCachedClip(string fileName, out AudioClip clip)
    {
        return _clipCache.TryGetValue(fileName, out clip);
    }

    public void LoadClipAsync(string fileName, Action<AudioClip> onLoaded)
    {
        if (_clipCache.TryGetValue(fileName, out var cached))
        {
            onLoaded?.Invoke(cached);
            return;
        }

        string address = $"{_addressPrefix}{fileName}";
        var handle = Addressables.LoadAssetAsync<AudioClip>(address);
        _loadHandles[fileName] = handle;

        handle.Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                _clipCache[fileName] = op.Result;
                onLoaded?.Invoke(op.Result);
            }
            else
            {
                Debug.LogWarning($"[AudioClipLoader] Failed to load audio clip: {fileName}");
                onLoaded?.Invoke(null);
            }
        };
    }

    public void ReleaseClip(string fileName)
    {
        if (_loadHandles.TryGetValue(fileName, out var handle))
        {
            Addressables.Release(handle);
            _loadHandles.Remove(fileName);
        }
        _clipCache.Remove(fileName);
    }

    public void ReleaseAllClips()
    {
        foreach (var handle in _loadHandles.Values)
        {
            Addressables.Release(handle);
        }
        _loadHandles.Clear();
        _clipCache.Clear();
    }
}
