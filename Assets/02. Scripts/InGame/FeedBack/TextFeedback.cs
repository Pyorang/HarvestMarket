using UnityEngine;



public class TextFeedback : MonoBehaviour, IClickFeedback
{
    [Header("스폰 위치")]
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] private Vector3 _spawnOffset = new Vector3(0f, 1f, 0f);

    [Header("랜덤 범위")]
    [SerializeField] private Vector2 _randomRangeX = new Vector2(-0.5f, 0.5f);
    [SerializeField] private Vector2 _randomRangeY = new Vector2(-0.2f, 0.2f);
    [SerializeField] private Vector2 _randomRangeZ = new Vector2(-0.5f, 0.5f);

    [Header("아이콘 오프셋 (텍스트 기준)")]
    [SerializeField] private Vector3 _iconOffset = new Vector3(0.5f, 0f, 0f);

    [Header("기즈모")]
    [SerializeField] private Color _gizmoColor = Color.yellow;

    private Farm _farm;

    private void Awake()
    {
        _farm = GetComponent<Farm>();
    }

    public void Play()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(_randomRangeX.x, _randomRangeX.y),
            Random.Range(_randomRangeY.x, _randomRangeY.y),
            Random.Range(_randomRangeZ.x, _randomRangeZ.y)
        );

        Debug.Log(randomOffset);

        Vector3 spawnPosition = _spawnLocation.position + _spawnOffset + randomOffset;

        var textObj = TextFeedbackPool.Instance.SpawnText(spawnPosition);

        var icon = TextFeedbackPool.Instance.SpawnIcon(
            _farm.Resource,
            _iconOffset,
            textObj.transform
        );

        if (textObj.TryGetComponent<ResourceGainText>(out var gainText))
        {
            gainText.SetIcon(icon);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_spawnLocation == null) return;

        Vector3 randomCenter = new Vector3(
            (_randomRangeX.x + _randomRangeX.y) * 0.5f,
            (_randomRangeY.x + _randomRangeY.y) * 0.5f,
            (_randomRangeZ.x + _randomRangeZ.y) * 0.5f
        );

        Vector3 center = _spawnLocation.position + _spawnOffset + randomCenter;

        Vector3 size = new Vector3(
            _randomRangeX.y - _randomRangeX.x,
            _randomRangeY.y - _randomRangeY.x,
            _randomRangeZ.y - _randomRangeZ.x
        );

        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireCube(center, size);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_spawnLocation.position + _spawnOffset, 0.05f);
    }
}

