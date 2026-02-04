using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerPrefsUpgradeRepository : IUpgradeRepository
{
    private readonly string _keyPrefix;
    private readonly string _existsKey;

    public PlayerPrefsUpgradeRepository(string userKey = "")
    {
        var prefix = string.IsNullOrEmpty(userKey) ? "" : userKey + "_";
        _keyPrefix = prefix + "Upgrade_";
        _existsKey = prefix + "Upgrade_Initialized";
    }

    public async UniTask<PlayerUpgradeData> Load()
    {
        await UniTask.Yield();
        
        var data = new PlayerUpgradeData();
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            data.SetLevel(type, PlayerPrefs.GetInt($"{_keyPrefix}{type}", 0));
        }
        return data;
    }

    public async UniTaskVoid Save(PlayerUpgradeData data)
    {
        await UniTask.Yield();
        
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            PlayerPrefs.SetInt($"{_keyPrefix}{type}", data.GetLevel(type));
        }
        PlayerPrefs.SetInt(_existsKey, 1);
        PlayerPrefs.Save();
    }

    public async UniTaskVoid Delete()
    {
        await UniTask.Yield();
        
        foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
        {
            PlayerPrefs.DeleteKey($"{_keyPrefix}{type}");
        }
        PlayerPrefs.DeleteKey(_existsKey);
        PlayerPrefs.Save();
    }

    public bool Exists() => PlayerPrefs.HasKey(_existsKey);
}
