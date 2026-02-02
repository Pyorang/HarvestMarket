using UnityEngine;

public class SoundFeedback : MonoBehaviour, IClickFeedback
{
    private const string _soundName = "Clicker";

    public void Play(ClickInfo clickInfo)
    {
        AudioManager.Instance.Play(AudioType.SFX, _soundName);
    }
}
