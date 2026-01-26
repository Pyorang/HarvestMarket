using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager Instance => s_instance;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private float _eggs;
    private float _meats;
    private float _milks;
    private float _golds;

    public float Eggs
    {
        get => _eggs;
        set => _eggs = Mathf.Max(0, value);
    }
    
    public float Meats
    {
        get => _meats;
        set => _meats = Mathf.Max(0, value);
    }

    public float Milks
    {
        get => _milks;
        set => _milks = Mathf.Max(0, value);
    }

    public float Gold
    {
        get => _golds;
        set => _golds = Mathf.Max(0, value);
    }


}