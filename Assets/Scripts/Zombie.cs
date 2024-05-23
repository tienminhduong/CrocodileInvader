using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : PoolableObject
{
    [Header("Zombie")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    public override float Width => boxCollider.size.x;

    private const int ZombieLayer0 = 5;

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
            renderer.sortingLayerName = "Zombie" + layer.ToString();

        gameObject.layer = layer + ZombieLayer0;
    }
}
