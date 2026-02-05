using System;
using System.Collections.Generic;
using UnityEngine;

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

    private readonly Dictionary<AudioType, IAudioChannel> _channels = new();
    private IAudioClipLoader _clipLoader;

    private const string BGM = "BGM";

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

    private void Start()
    {
        Play(AudioType.BGM, BGM);
    }

    private void Init()
    {
        _clipLoader = new AddressableAudioClipLoader();

        _channels[AudioType.BGM] = new BGMChannel(transform, _clipLoader);
        _channels[AudioType.SFX] = new SFXChannel(transform, _clipLoader);
    }

    private IAudioChannel GetChannel(AudioType audioType)
    {
        if (_channels.TryGetValue(audioType, out var channel))
            return channel;

        Debug.LogWarning($"[AudioManager] Channel not found: {audioType}");
        return null;
    }

    public void Play(AudioType audioType, string fileName)
    {
        GetChannel(audioType)?.Play(fileName);
    }

    public void PlayWithCallback(AudioType audioType, string fileName, Action onComplete)
    {
        var channel = GetChannel(audioType);
        if (channel != null)
            channel.PlayWithCallback(fileName, onComplete);
        else
            onComplete?.Invoke();
    }

    public float GetVolume(AudioType audioType)
    {
        return GetChannel(audioType)?.Volume ?? 0f;
    }

    public void SetVolume(AudioType audioType, float volume)
    {
        var channel = GetChannel(audioType);
        if (channel != null) channel.Volume = volume;
    }

    public void SetPitch(AudioType audioType, float pitch)
    {
        var channel = GetChannel(audioType);
        if (channel != null) channel.Pitch = pitch;
    }

    public void Pause(AudioType audioType) => GetChannel(audioType)?.Pause();
    public void Resume(AudioType audioType) => GetChannel(audioType)?.Resume();
    public void Stop(AudioType audioType) => GetChannel(audioType)?.Stop();

    public bool IsPlaying(AudioType audioType)
    {
        return GetChannel(audioType)?.IsPlaying ?? false;
    }

    public void StopAll()
    {
        foreach (var channel in _channels.Values) channel.Stop();
    }

    public void Mute()
    {
        foreach (var channel in _channels.Values) channel.Mute();
    }

    public void Unmute()
    {
        foreach (var channel in _channels.Values) channel.Unmute();
    }

    public void PreloadClip(string fileName, Action<bool> onComplete = null)
    {
        if (_clipLoader.TryGetCachedClip(fileName, out _))
        {
            onComplete?.Invoke(true);
            return;
        }

        _clipLoader.LoadClipAsync(fileName, clip => onComplete?.Invoke(clip != null));
    }

    public void PreloadClips(string[] fileNames, Action onAllComplete = null)
    {
        int remaining = fileNames.Length;

        foreach (var fileName in fileNames)
        {
            PreloadClip(fileName, _ =>
            {
                remaining--;
                if (remaining <= 0) onAllComplete?.Invoke();
            });
        }
    }

    public void ReleaseClip(string fileName) => _clipLoader.ReleaseClip(fileName);
    public void ReleaseAllClips() => _clipLoader.ReleaseAllClips();

    private void OnDestroy()
    {
        ReleaseAllClips();

        if (s_instance == this)
        {
            s_instance = null;
        }
    }
}
