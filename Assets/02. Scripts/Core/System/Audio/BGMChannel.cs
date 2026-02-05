using System;
using UnityEngine;

public class BGMChannel : IAudioChannel
{
    private readonly AudioSource _source;
    private readonly IAudioClipLoader _loader;

    public BGMChannel(Transform parent, IAudioClipLoader loader)
    {
        _loader = loader;

        var go = new GameObject("BGM");
        go.transform.SetParent(parent);
        _source = go.AddComponent<AudioSource>();
        _source.loop = true;
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
            if (clip == null) return;

            if (_source.isPlaying) _source.Stop();
            _source.clip = clip;
            _source.Play();
        });
    }

    public void PlayWithCallback(string fileName, Action onComplete)
    {
        _loader.LoadClipAsync(fileName, clip =>
        {
            if (clip != null)
            {
                if (_source.isPlaying) _source.Stop();
                _source.clip = clip;
                _source.Play();
            }
            onComplete?.Invoke();
        });
    }

    public void Pause() => _source.Pause();
    public void Resume() => _source.UnPause();
    public void Stop() => _source.Stop();
    public void Mute() => _source.mute = true;
    public void Unmute() => _source.mute = false;
}
