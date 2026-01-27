using UnityEngine;

public class SoundFeedback : MonoBehaviour, IClickFeedback
{
    [SerializeField] private string _soundName = "Clicker";

    public void Play()
    {
        AudioManager.Instance.Play(AudioType.SFX, _soundName);
    }
}
