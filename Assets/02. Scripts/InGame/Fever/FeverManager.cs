using System;
using UnityEngine;

public class FeverManager : MonoBehaviour
{
    private static FeverManager s_instance;
    public static FeverManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                var go = new GameObject("FeverManager");
                s_instance = go.AddComponent<FeverManager>();
            }
            return s_instance;
        }
    }

    [Header("피버 설정")]
    [SerializeField] private float _maxGauge = 100f;
    [SerializeField] private float _feverDuration = 10f;
    [SerializeField] private float _bonusMultiplier = 2f;

    [Header("게이지 감소 설정")]
    [SerializeField] private float _decayDelay = 3f;
    [SerializeField] private float _decayAmount = 5f;

    [Header("스포트라이트")]
    [SerializeField] private GameObject[] _spotLightParents;

    private const string FEVER_BGM = "Fever";
    private const string NORMAL_BGM = "BGM";

    private float _currentGauge;
    private bool _isFeverActive;
    private float _feverTimer;
    private float _lastClickTime;
    private float _decayTimer;

    public static event Action<float, float> OnGaugeChanged;
    public static event Action<bool> OnFeverStateChanged;

    public bool IsFeverActive => _isFeverActive;
    public float BonusMultiplier => _isFeverActive ? _bonusMultiplier : 1f;

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

    private void Start()
    {
        _currentGauge = 0f;
        _lastClickTime = -_decayDelay;
        OnGaugeChanged?.Invoke(_currentGauge, _maxGauge);
    }

    private void Update()
    {
        if (_isFeverActive)
        {
            UpdateFeverTimer();
        }
        else
        {
            DecayGauge();
        }
    }

    public void AddGauge(float amount)
    {
        if (_isFeverActive) return;

        _currentGauge = Mathf.Min(_currentGauge + amount, _maxGauge);
        _lastClickTime = Time.time;
        _decayTimer = 0f;
        OnGaugeChanged?.Invoke(_currentGauge, _maxGauge);

        if (_currentGauge >= _maxGauge)
        {
            ActivateFever();
        }
    }

    private void ActivateFever()
    {
        _isFeverActive = true;
        _feverTimer = _feverDuration;
        _currentGauge = _maxGauge;
        
        AudioManager.Instance.Play(AudioType.BGM, FEVER_BGM);
        SkyBoxController.Instance.TransitionToFever();
        SetSpotLightsActive(true);
        
        OnFeverStateChanged?.Invoke(true);
    }

    private void DeactivateFever()
    {
        _isFeverActive = false;
        _currentGauge = 0f;
        _lastClickTime = Time.time;
        
        AudioManager.Instance.Play(AudioType.BGM, NORMAL_BGM);
        SkyBoxController.Instance.TransitionToNormal();
        SetSpotLightsActive(false);
        
        OnGaugeChanged?.Invoke(_currentGauge, _maxGauge);
        OnFeverStateChanged?.Invoke(false);
    }

    private void UpdateFeverTimer()
    {
        _feverTimer -= Time.deltaTime;

        // 피버 중 게이지가 서서히 줄어듦
        _currentGauge = (_feverTimer / _feverDuration) * _maxGauge;
        OnGaugeChanged?.Invoke(_currentGauge, _maxGauge);

        if (_feverTimer <= 0f)
        {
            DeactivateFever();
        }
    }

    private void DecayGauge()
    {
        if (_currentGauge <= 0f) return;
        if (Time.time - _lastClickTime < _decayDelay) return;

        _decayTimer += Time.deltaTime;

        if (_decayTimer >= 1f)
        {
            _currentGauge = Mathf.Max(0f, _currentGauge - _decayAmount);
            _decayTimer = 0f;
            OnGaugeChanged?.Invoke(_currentGauge, _maxGauge);
        }
    }

    private void SetSpotLightsActive(bool active)
    {
        foreach (var spotLight in _spotLightParents)
        {
            if (spotLight != null)
            {
                spotLight.SetActive(active);
            }
        }
    }
}
