using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum AudioType
{
    BGM,
    SFX
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager s_instance;
    public static AudioManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                var go = new GameObject("AudioManager");
                s_instance = go.AddComponent<AudioManager>();
                DontDestroyOnLoad(go);
            }
            return s_instance;
        }
    }

    private AudioSource _bgmSource;
    private AudioSource _sfxSource;
    
    private readonly Dictionary<string, AudioClip> _clipCache = new();
    private readonly Dictionary<string, AsyncOperationHandle<AudioClip>> _loadHandles = new();

    private const string AUDIO_ADDRESS_PREFIX = "Audio/";

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        CreateBGMSource();
        CreateSFXSource();
    }

    private void CreateBGMSource()
    {
        var bgmObject = new GameObject("BGM");
        bgmObject.transform.SetParent(transform);
        _bgmSource = bgmObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _bgmSource.playOnAwake = false;
    }

    private void CreateSFXSource()
    {
        var sfxObject = new GameObject("SFX");
        sfxObject.transform.SetParent(transform);
        _sfxSource = sfxObject.AddComponent<AudioSource>();
        _sfxSource.playOnAwake = false;
    }
    
    public void Play(AudioType audioType, string fileName)
    {
        if (_clipCache.TryGetValue(fileName, out var clip))
        {
            PlayInternal(audioType, clip);
            return;
        }

        LoadClipAsync(fileName, loadedClip =>
        {
            if (loadedClip != null)
            {
                PlayInternal(audioType, loadedClip);
            }
        });
    }

    public void PlayWithCallback(AudioType audioType, string fileName, Action onComplete)
    {
        if (_clipCache.TryGetValue(fileName, out var clip))
        {
            PlayInternal(audioType, clip);
            onComplete?.Invoke();
            return;
        }

        LoadClipAsync(fileName, loadedClip =>
        {
            if (loadedClip != null)
            {
                PlayInternal(audioType, loadedClip);
            }
            onComplete?.Invoke();
        });
    }

    private void PlayInternal(AudioType audioType, AudioClip clip)
    {
        switch (audioType)
        {
            case AudioType.BGM:
                if (_bgmSource.isPlaying)
                {
                    _bgmSource.Stop();
                }
                _bgmSource.clip = clip;
                _bgmSource.Play();
                break;
                
            case AudioType.SFX:
                _sfxSource.PlayOneShot(clip);
                break;
        }
    }
    
    private void LoadClipAsync(string fileName, Action<AudioClip> onLoaded)
    {
        string address = $"{AUDIO_ADDRESS_PREFIX}{fileName}";
        
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
                Debug.LogWarning($"[AudioManager] Failed to load audio clip: {fileName}");
                onLoaded?.Invoke(null);
            }
        };
    }

    public void PreloadClip(string fileName, Action<bool> onComplete = null)
    {
        if (_clipCache.ContainsKey(fileName))
        {
            onComplete?.Invoke(true);
            return;
        }

        LoadClipAsync(fileName, clip =>
        {
            onComplete?.Invoke(clip != null);
        });
    }

    public void PreloadClips(string[] fileNames, Action onAllComplete = null)
    {
        int remaining = fileNames.Length;
        
        foreach (var fileName in fileNames)
        {
            PreloadClip(fileName, success =>
            {
                remaining--;
                if (remaining <= 0)
                {
                    onAllComplete?.Invoke();
                }
            });
        }
    }
    
    public float GetVolume(AudioType audioType)
    {
        return audioType switch
        {
            AudioType.BGM => _bgmSource.volume,
            AudioType.SFX => _sfxSource.volume,
            _ => 0f
        };
    }

    public void SetVolume(AudioType audioType, float volume)
    {
        volume = Mathf.Clamp01(volume);
        
        switch (audioType)
        {
            case AudioType.BGM:
                _bgmSource.volume = volume;
                break;
            case AudioType.SFX:
                _sfxSource.volume = volume;
                break;
        }
    }

    public void SetPitch(AudioType audioType, float pitch)
    {
        switch (audioType)
        {
            case AudioType.BGM:
                _bgmSource.pitch = pitch;
                break;
            case AudioType.SFX:
                _sfxSource.pitch = pitch;
                break;
        }
    }
    
    public void Pause(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.BGM:
                _bgmSource.Pause();
                break;
            case AudioType.SFX:
                _sfxSource.Pause();
                break;
        }
    }

    public void Resume(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.BGM:
                _bgmSource.UnPause();
                break;
            case AudioType.SFX:
                _sfxSource.UnPause();
                break;
        }
    }

    public void Stop(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.BGM:
                _bgmSource.Stop();
                break;
            case AudioType.SFX:
                _sfxSource.Stop();
                break;
        }
    }

    public void StopAll()
    {
        _bgmSource.Stop();
        _sfxSource.Stop();
    }

    public void Mute()
    {
        _bgmSource.mute = true;
        _sfxSource.mute = true;
    }

    public void Unmute()
    {
        _bgmSource.mute = false;
        _sfxSource.mute = false;
    }

    public bool IsPlaying(AudioType audioType)
    {
        return audioType switch
        {
            AudioType.BGM => _bgmSource.isPlaying,
            AudioType.SFX => _sfxSource.isPlaying,
            _ => false
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

    private void OnDestroy()
    {
        ReleaseAllClips();
        
        if (s_instance == this)
        {
            s_instance = null;
        }
    }
}
