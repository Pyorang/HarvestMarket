using System;
using UnityEngine;

public class SFXChannel : IAudioChannel
{
    private readonly AudioSource _source;
    private readonly IAudioClipLoader _loader;

    public SFXChannel(Transform parent, IAudioClipLoader loader)
    {
        _loader = loader;

        var go = new GameObject("SFX");
        go.transform.SetParent(parent);
        _source = go.AddComponent<AudioSource>();
        _source.playOnAwake = false;
    }

    public bool IsPlaying => _source.isPlaying;

    public float Volume
    {
        get => _source.volume;
        set => _source.volume = Mathf.Clamp01(value);
    }

    public float Pitch
    {
        get => _source.pitch;
        set => _source.pitch = value;
    }

    public void Play(string fileName)
    {
        _loader.LoadClipAsync(fileName, clip =>
        {
            if (clip != null) _source.PlayOneShot(clip);
        });
    }

    public void PlayWithCallback(string fileName, Action onComplete)
    {
        _loader.LoadClipAsync(fileName, clip =>
        {
            if (clip != null) _source.PlayOneShot(clip);
            onComplete?.Invoke();
        });
    }

    public void Pause() => _source.Pause();
    public void Resume() => _source.UnPause();
    public void Stop() => _source.Stop();
    public void Mute() => _source.mute = true;
    public void Unmute() => _source.mute = false;
}
