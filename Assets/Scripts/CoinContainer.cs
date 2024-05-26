using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinContainer : PoolableObject
{
    [SerializeField] private BoxCollider2D boxCollider;

    public override float Width => boxCollider.size.x;
    public override float Height => boxCollider.size.y;
    public override void Init()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
