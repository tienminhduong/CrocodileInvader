using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseZombie : Zombie
{
    [SerializeField] private Animator legAnimator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        legAnimator.SetBool("isOnGround", JumpStatus == 0);
    }
}
