using UnityEngine;

public class FeverBGMHandler : MonoBehaviour
{
    [SerializeField] private string _feverBGM = "Fever";
    [SerializeField] private string _normalBGM = "BGM";

    private void OnEnable()
    {
        FeverManager.OnFeverStateChanged += OnFeverStateChanged;
    }

    private void OnDisable()
    {
        FeverManager.OnFeverStateChanged -= OnFeverStateChanged;
    }

    private void OnFeverStateChanged(bool isFever)
    {
        string bgm = isFever ? _feverBGM : _normalBGM;
        AudioManager.Instance.Play(AudioType.BGM, bgm);
    }
}
