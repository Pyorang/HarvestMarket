using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                var go = new GameObject("GameManager");
                s_instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return s_instance;
        }
    }

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
