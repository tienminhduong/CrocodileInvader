using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : PoolableObject
{
    [SerializeField] private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private float jumpForce;

    private float countTime;
    const float deltaJumpTime = 1f;

    public override float Height => boxCollider.size.y;
    public override float Width => boxCollider.size.x;

    const int HumanLayer0 = 11;

    protected override void Update()
    {
        base.Update();
        JumpingAnimation();
    }

    private void JumpingAnimation()
    {
        countTime += Time.deltaTime;
        if (countTime >= deltaJumpTime)
        {
            //transform.position += Vector3.up * GameManager.ScreenHeight / 25f;
            rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            countTime -= deltaJumpTime;
        }
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
