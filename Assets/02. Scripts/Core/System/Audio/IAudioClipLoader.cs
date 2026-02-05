using System;
using UnityEngine;

public interface IAudioClipLoader
{
    void LoadClipAsync(string fileName, Action<AudioClip> onLoaded);
    void ReleaseClip(string fileName);
    void ReleaseAllClips();
    bool TryGetCachedClip(string fileName, out AudioClip clip);
}
