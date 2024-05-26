using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : Manager
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void CallSpawnItem()
    {
        bool isSpawn4 = Random.Range(0, 10) == 0;
        if (isSpawn4)
        {
            for (int i = 0; i < 4; ++i)
            {
                Human h = (Human)GetItem(Random.Range(0, PrefabsCount));
                h.transform.position += Vector3.right * (i * h.Width);
                h.SetLayer(i == 3 ? 3 : 3 - i);
            }
        }
        else
        {
            Human h = (Human)GetItem(Random.Range(0, PrefabsCount));
            h.SetLayer(Random.Range(1, 4));
        }
    }
}
