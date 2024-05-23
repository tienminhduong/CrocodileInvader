using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : PoolableObject
{
    [Header("Road")]
    [SerializeField] private Surface surface;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;

    public override float Width => spriteRenderer.size.x;

    // Start is called before the first frame update
    protected override void Start()
    {
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void Config(float width, int layer)
    {
        spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);
        surface.Resize(width);
        boxCollider.size = new Vector2(width, boxCollider.size.y);

        spriteRenderer.sortingOrder = layer;
    }
}
