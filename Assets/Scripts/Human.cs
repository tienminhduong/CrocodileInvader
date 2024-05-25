using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : PoolableObject
{
    [SerializeField] private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    [SerializeField] private BoxCollider2D boxCollider;

    public override float Height => boxCollider.size.y;
    public override float Width => boxCollider.size.x;

    const int HumanLayer0 = 11;

    public override void Init()
    {
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

    public void SetLayer(int layer)
    {
        foreach (SpriteRenderer renderer in renderers)
            renderer.sortingLayerName = "Layer" + layer.ToString();

        gameObject.layer = layer + HumanLayer0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            RemoveSelf();
            GameManager.Instance.GenerateZombies();
        }
    }
}
