using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseUpgradeRepository : IUpgradeRepository
{
    private const string UPGRADE_COLLECTION_NAME = "Upgrade";
    
    private FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;
    
    public async UniTaskVoid Save(PlayerUpgradeData upgradeData)
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            if (string.IsNullOrEmpty(email))
            {
                Debug.LogError("User email is null or empty");
                return;
            }
            
            await _db.Collection(UPGRADE_COLLECTION_NAME).Document(email).SetAsync(upgradeData);
            Debug.Log("Upgrade 데이터 Firebase 저장 성공");
        }
        catch (Exception e)
        {
            Debug.LogError("Upgrade Firebase 저장 실패: " + e.Message);
        }
    }
    
    public async UniTask<PlayerUpgradeData> Load()
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            if (string.IsNullOrEmpty(email))
            {
                Debug.LogError("User email is null or empty");
                return GetDefaultUpgradeData();
            }
            
            DocumentSnapshot snapshot = await _db.Collection(UPGRADE_COLLECTION_NAME)
                .Document(email).GetSnapshotAsync();
                
            if (snapshot.Exists)
            {
                var data = snapshot.ConvertTo<PlayerUpgradeData>();
                Debug.Log("Upgrade 데이터 Firebase 로드 성공");
                return data;
            }
            else
            {
                Debug.Log("Upgrade 데이터가 존재하지 않아 기본값 반환");
                return GetDefaultUpgradeData();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Upgrade Firebase 로드 실패: " + e.Message);
            return GetDefaultUpgradeData();
        }
    }
    
    private PlayerUpgradeData GetDefaultUpgradeData()
    {
        var data = new PlayerUpgradeData();
        data.SetDefault();
        return data;
    }
}