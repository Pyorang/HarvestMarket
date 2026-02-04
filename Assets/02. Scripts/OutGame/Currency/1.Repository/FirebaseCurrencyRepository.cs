using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseCurrencyRepository : ICurrencyRepository
{
    private const string CURRENCY_COLLECTION_NAME = "Currency";
    
    private FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;
    
    public async UniTaskVoid Save(CurrencyData currencyData)
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            if (string.IsNullOrEmpty(email))
            {
                Debug.LogError("User email is null or empty");
                return;
            }
            
            await _db.Collection(CURRENCY_COLLECTION_NAME).Document(email).SetAsync(currencyData);
            Debug.Log("Currency 데이터 Firebase 저장 성공");
        }
        catch (Exception e)
        {
            Debug.LogError("Currency Firebase 저장 실패: " + e.Message);
        }
    }
    
    public async UniTask<CurrencyData> Load()
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            if (string.IsNullOrEmpty(email))
            {
                Debug.LogError("User email is null or empty");
                return GetDefaultCurrencyData();
            }
            
            DocumentSnapshot snapshot = await _db.Collection(CURRENCY_COLLECTION_NAME)
                .Document(email).GetSnapshotAsync();
                
            if (snapshot.Exists)
            {
                var data = snapshot.ConvertTo<CurrencyData>();
                Debug.Log("Currency 데이터 Firebase 로드 성공");
                return data;
            }
            else
            {
                Debug.Log("Currency 데이터가 존재하지 않아 기본값 반환");
                return GetDefaultCurrencyData();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Currency Firebase 로드 실패: " + e.Message);
            return GetDefaultCurrencyData();
        }
    }
    
    private CurrencyData GetDefaultCurrencyData()
    {
        var data = new CurrencyData();
        data.SetDefault();
        return data;
    }
}