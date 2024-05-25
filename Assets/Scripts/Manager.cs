using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private int numberRepool;

    [SerializeField] List<PoolableObject> objectPrefabs = new List<PoolableObject>();
    private Dictionary<int, Queue<PoolableObject>> pooling;

    public int PrefabsCount => objectPrefabs.Count;

    protected virtual void Init(int id = 0)
    {
        if (pooling == null)
        {
            pooling = new Dictionary<int, Queue<PoolableObject>>();
            for (int i = 0; i < objectPrefabs.Count; i++)
                pooling[i] = new Queue<PoolableObject>();
        }
        pooling[id] ??= new Queue<PoolableObject>();

        for (int i = 0; i < numberRepool; i++)
        {
            PoolableObject p = Instantiate(objectPrefabs[id], transform);
            p.gameObject.SetActive(false);
            p.AssociatedManager = this;
            pooling[id].Enqueue(p);
        }
    }

    public virtual PoolableObject GetItem(int id = 0)
    {
        if (pooling[id].Count <= 0)
            Init(id);

        PoolableObject p = pooling[id].Dequeue();
        p.gameObject.SetActive(true);
        p.Init();
        return p;
    }

    public void ReturnItem(PoolableObject item)
    {
        item.gameObject.SetActive(false);
        pooling[item.ID].Enqueue(item);
    }

    public virtual void CallSpawnItem()
    {
    }

    private void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
    }
}
