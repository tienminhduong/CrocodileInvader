using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : PoolableObject
{
    [SerializeField] private BoxCollider2D boxCollider;

    public override float Width => boxCollider.size.x;
    public override float Height => boxCollider.size.y;

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            RemoveSelf();
            if (GameManager.Instance.Zombies.CurrentFormID == 1)
                GameManager.Instance.Coins.TranformIntoCoin(this, true, 0);
        }
        else if (!collision.gameObject.CompareTag("Road"))
            RemoveSelf();
    }
}
