using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : Manager
{
    public override PoolableObject GetItem(int id = 0)
    {
        Human h = (Human)base.GetItem(id);
        h.transform.position = GameManager.Instance.SpawnerPosition;
        h.Init();

        return h;
    }

    [SerializeField] private float t;

    // Start is called before the first frame update
    void Start()
    {
        t = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t > 3f)
        {
            GetItem(Random.Range(0, PrefabsCount));
            t = 0f;
        }
    }
}
