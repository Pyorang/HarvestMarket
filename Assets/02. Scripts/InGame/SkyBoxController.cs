using UnityEngine;
using DG.Tweening;

public class SkyBoxController : MonoBehaviour
{
    private static SkyBoxController s_instance;
    public static SkyBoxController Instance
    {
        get
        {
            if (s_instance == null)
            {
                var go = new GameObject("SkyBoxController");
                s_instance = go.AddComponent<SkyBoxController>();
            }
            return s_instance;
        }
    }

    [Header("스카이박스")]
    [SerializeField] private Material _skyboxMaterial;

    [Header("일반 상태 색상")]
    [SerializeField] private Color _normalColorTop = Color.cyan;
    [SerializeField] private Color _normalColorMiddle = Color.white;
    [SerializeField] private Color _normalColorBottom = Color.gray;

    [Header("피버 상태 색상")]
    [SerializeField] private Color _feverColorTop = Color.red;
    [SerializeField] private Color _feverColorMiddle = Color.yellow;
    [SerializeField] private Color _feverColorBottom = Color.magenta;

    [Header("Directional Light")]
    [SerializeField] private Light _directionalLight;
    [SerializeField] private float _normalIntensity = 1f;
    [SerializeField] private float _feverIntensity = 1.5f;

    [Header("전환 설정")]
    [SerializeField] private float _transitionDuration = 0.5f;

    private static readonly int ColorTop = Shader.PropertyToID("_TopColor");
    private static readonly int ColorMiddle = Shader.PropertyToID("_MiddleColor");
    private static readonly int ColorBottom = Shader.PropertyToID("_BottomColor");

    private Sequence _colorSequence;
    private Tween _lightTween;

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
        SetNormalImmediate();
    }

    public void TransitionToNormal()
    {
        TransitionColors(_normalColorTop, _normalColorMiddle, _normalColorBottom);
        TransitionLight(_normalIntensity);
    }

    public void TransitionToFever()
    {
        TransitionColors(_feverColorTop, _feverColorMiddle, _feverColorBottom);
        TransitionLight(_feverIntensity);
    }

    public void SetNormalImmediate()
    {
        _skyboxMaterial.SetColor(ColorTop, _normalColorTop);
        _skyboxMaterial.SetColor(ColorMiddle, _normalColorMiddle);
        _skyboxMaterial.SetColor(ColorBottom, _normalColorBottom);
        
        if (_directionalLight != null)
        {
            _directionalLight.intensity = _normalIntensity;
        }
    }

    private void TransitionColors(Color top, Color middle, Color bottom)
    {
        _colorSequence?.Kill();
        _colorSequence = DOTween.Sequence();

        _colorSequence.Append(
            DOTween.To(
                () => _skyboxMaterial.GetColor(ColorTop),
                x => _skyboxMaterial.SetColor(ColorTop, x),
                top,
                _transitionDuration
            )
        );

        _colorSequence.Join(
            DOTween.To(
                () => _skyboxMaterial.GetColor(ColorMiddle),
                x => _skyboxMaterial.SetColor(ColorMiddle, x),
                middle,
                _transitionDuration
            )
        );

        _colorSequence.Join(
            DOTween.To(
                () => _skyboxMaterial.GetColor(ColorBottom),
                x => _skyboxMaterial.SetColor(ColorBottom, x),
                bottom,
                _transitionDuration
            )
        );
    }

    private void TransitionLight(float targetIntensity)
    {
        if (_directionalLight == null) return;

        _lightTween?.Kill();
        _lightTween = DOTween.To(
            () => _directionalLight.intensity,
            x => _directionalLight.intensity = x,
            targetIntensity,
            _transitionDuration
        );
    }

    private void OnDisable()
    {
        _colorSequence?.Kill();
        _lightTween?.Kill();
    }
}
