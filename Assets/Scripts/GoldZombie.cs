using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldZombie : Zombie
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetJumpHeight(1.2f);
    }

    // Update is called once per frame
    //protected override void Update()
    //{

    //}
    protected override void OnCollisionEnterObjectBehavior(Collision2D collision)
    {
    }
}
