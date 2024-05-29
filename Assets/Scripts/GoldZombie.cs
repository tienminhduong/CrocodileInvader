using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldZombie : Zombie
{
    [SerializeField] private Animator leftLegAnimator, rightLegAnimator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetJumpHeight(1.2f);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // animation controller
        leftLegAnimator.SetBool("isOnGround", JumpStatus == 0);
        rightLegAnimator.SetBool("isOnGround", JumpStatus == 0);
    }

    protected override void OnCollisionEnterObjectBehavior(Collision2D collision)
    {
    }
}
