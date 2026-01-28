using System;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    private static FarmManager s_instance;
    public static FarmManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                var go = new GameObject("FarmManager");
                s_instance = go.AddComponent<FarmManager>();
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
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    [Header("농장 별 카메라 위치")]
    [SerializeField] private Transform[] _farmCameraView;

    [Header("카메라")]
    [SerializeField] private CameraRubberBand _cameraRubberBand;

    private int _currentFarmIndex;
    private int _maxFarmIndex;

    public static event Action<int, bool, bool> OnFarmIndexChanged;

    public int CurrentFarmIndex => _currentFarmIndex;

    private void Start()
    {
        _currentFarmIndex = 0;
        _maxFarmIndex = _farmCameraView.Length - 1;

        NotifyIndexChanged();
    }

    public void MoveToNextFarm()
    {
        if (_currentFarmIndex >= _maxFarmIndex) return;

        _currentFarmIndex++;
        MoveCameraToCurrentFarm();
        NotifyIndexChanged();
    }

    public void MoveToPreviousFarm()
    {
        if (_currentFarmIndex <= 0) return;

        _currentFarmIndex--;
        MoveCameraToCurrentFarm();
        NotifyIndexChanged();
    }

    private void MoveCameraToCurrentFarm()
    {
        var target = _farmCameraView[_currentFarmIndex];
        _cameraRubberBand.Shoot(target);
    }

    private void NotifyIndexChanged()
    {
        bool isAtMin = _currentFarmIndex <= 0;
        bool isAtMax = _currentFarmIndex >= _maxFarmIndex;
        OnFarmIndexChanged?.Invoke(_currentFarmIndex, isAtMin, isAtMax);
    }
}

