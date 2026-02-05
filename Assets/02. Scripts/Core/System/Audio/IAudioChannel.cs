using System;

public interface IAudioChannel
{
    void Play(string fileName);
    void PlayWithCallback(string fileName, Action onComplete);
    void Pause();
    void Resume();
    void Stop();
    void Mute();
    void Unmute();
    bool IsPlaying { get; }
    float Volume { get; set; }
    float Pitch { get; set; }
}
