using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemManager : Manager
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
        GetItem(Random.Range(0, 2));
    }

    public void TranformIntoCoin(PoolableObject item, bool isBomb, int vehicleID)
    {
        int ID = 3;
        if (!isBomb)
            ID += vehicleID + 1;
        GemContainer cc = (GemContainer)GetItem(ID);
        cc.transform.position = item.transform.position;
    }
}
