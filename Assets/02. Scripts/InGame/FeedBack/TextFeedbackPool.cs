using UnityEngine;
using System.Collections.Generic;
using Lean.Pool;

public class TextFeedbackPool : MonoBehaviour
{
    public static TextFeedbackPool Instance { get; private set; }

    [System.Serializable]
    public class PoolEntry
    {
        public ResourceType resourceType;
        public LeanGameObjectPool pool;
    }

    [Header("텍스트 풀")]
    [SerializeField] private LeanGameObjectPool _textPool;

    [Header("리소스 아이콘 풀")]
    [SerializeField] private PoolEntry[] _iconPoolEntries;

    private Dictionary<ResourceType, LeanGameObjectPool> _iconPoolDict;

    private void Awake()
    {
        Instance = this;

        _iconPoolDict = new Dictionary<ResourceType, LeanGameObjectPool>();
        foreach (var entry in _iconPoolEntries)
        {
            _iconPoolDict[entry.resourceType] = entry.pool;
        }
    }

    public GameObject SpawnText(Vector3 position)
    {
        return _textPool.Spawn(position, Quaternion.identity);
    }

    public GameObject SpawnIcon(ResourceType type, Vector3 localPosition, Transform parent)
    {
        if (_iconPoolDict.TryGetValue(type, out var pool))
        {
            var obj = pool.Spawn(Vector3.zero, Quaternion.identity, parent);
            obj.transform.localPosition = localPosition;
            return obj;
        }

        Debug.LogWarning($"Pool not found for ResourceType: {type}");
        return null;
    }

    public void Despawn(GameObject obj)
    {
        LeanPool.Despawn(obj);
    }
}
