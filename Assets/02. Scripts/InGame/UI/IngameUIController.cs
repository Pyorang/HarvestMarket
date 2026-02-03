using UnityEngine;
using UnityEngine.UI;

public class IngameUIController : MonoBehaviour
{
    [Header("UI References")]
    [Space]
    [SerializeField] private GameObject _upgradePopUpPanel;

    public void OpenUpgrade()
    {
        _upgradePopUpPanel.SetActive(true);
    }

    public void CloseUpgrade()
    {
        _upgradePopUpPanel.SetActive(false);
    }
}
