using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance { get; private set; }

    public bool ExistsSavedData { get; private set; } = false;
    public List<IUserData> UserDataList { get; private set; } = new();

    private void Awake()
    {
        Instance = this;

        ExistsSavedData = PlayerPrefs.GetInt(nameof(ExistsSavedData)) == 1;

        UserDataList.Add(new ResourceData());

        if (ExistsSavedData)
        {
            LoadUserData();
        }
        else
        {
            SetDefaultData();
        }
    }

    public void SetDefaultData()
    {
        foreach (var data in UserDataList)
        {
            data.SetDefaultData();
        }
    }

    public void LoadUserData()
    {
        if (ExistsSavedData)
        {
            foreach (var data in UserDataList)
            {
                data.LoadData();
            }
        }
    }

    public void SaveUserData()
    {
        bool hasError = false;

        foreach (var data in UserDataList)
        {
            if (false == data.SaveData())
            {
                hasError = true;
                break;
            }

            if (hasError == false)
            {
                PlayerPrefs.SetInt(nameof(ExistsSavedData), 1);
                PlayerPrefs.Save();

                ExistsSavedData = true;
            }
        }
    }

    public T GetUserData<T>() where T : class, IUserData
    {
        return UserDataList.OfType<T>().FirstOrDefault();
    }
}
