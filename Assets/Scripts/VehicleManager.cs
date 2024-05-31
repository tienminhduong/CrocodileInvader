using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : Manager
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
        int rd = Random.Range(0, 100);
        int spawnID;
        if (GameManager.Instance.Zombies.Count < 8)
        {
            if (rd < 70)
                spawnID = 0;
            else if (rd < 70 + 15)
                spawnID = 1;
            else
                spawnID = 2;
        }
        else if (GameManager.Instance.Zombies.Count < 12)
        {
            if (rd < 70)
                spawnID = 1;
            else if (rd < 70 + 15)
                spawnID = 0;
            else
                spawnID = 2;
        }
        else
        {
            if (rd < 70)
                spawnID = 2;
            else if (rd < 70 + 15)
                spawnID = 1;
            else
                spawnID = 1;
        }
        GetItem(spawnID);
    }
}
