using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance { get; private set; }

    private IRepository<ResourceData> _resourceRepository;
    private IRepository<PlayerUpgradeData> _upgradeRepository;

    public ResourceData ResourceData { get; private set; }
    public PlayerUpgradeData UpgradeData { get; private set; }

    private void Awake()
    {
        Instance = this;

        _resourceRepository = new ResourceRepository();
        _upgradeRepository = new UpgradeRepository();

        if (_resourceRepository.Exists())
        {
            LoadAll();
        }
        else
        {
            SetDefaultAll();
        }
    }

    public void SetDefaultAll()
    {
        ResourceData = new ResourceData();
        ResourceData.SetDefault();

        UpgradeData = new PlayerUpgradeData();
        UpgradeData.SetDefault();
    }

    public void LoadAll()
    {
        ResourceData = _resourceRepository.Load();
        UpgradeData = _upgradeRepository.Load();
    }

    public void SaveAll()
    {
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            ResourceData.Resources[type] = (float)ResourceManager.Instance.GetResource(type);
        }

        _resourceRepository.Save(ResourceData);
        _upgradeRepository.Save(UpgradeData);
    }

    public void SaveResource()
    {
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            ResourceData.Resources[type] = (float)ResourceManager.Instance.GetResource(type);
        }
        _resourceRepository.Save(ResourceData);
    }

    public void SaveUpgrade()
    {
        _upgradeRepository.Save(UpgradeData);
    }

    public void DeleteAll()
    {
        _resourceRepository.Delete();
        _upgradeRepository.Delete();
        SetDefaultAll();
    }
}
