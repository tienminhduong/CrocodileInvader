using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : PoolableObject
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject sprite;
    private bool turnedGold = false;
    private bool collided;
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
        if (!turnedGold && GameManager.Instance.Zombies.FirstCrocodile &&
            (transform.position - GameManager.Instance.Zombies.FirstCrocodile.transform.position).magnitude
            <= GameManager.ScreenWidth * 0.2f &&
            GameManager.Instance.Zombies.CurrentFormID == Crocodile.SPELLCASTERID)
            TurnIntoGold();
    }

    public override void Init()
    {
        base.Init();
        turnedGold = false;
        boxCollider.isTrigger = false;
        collided = false;
    }

    private void TurnIntoGold()
    {
        GameManager.Instance.Coins.TranformIntoCoin(this, true, 0);
        GameManager.Instance.Zombies.FirstCrocodile.PlayAttackAnimation();
        GameplayMusicManager.Instance.PlayGoldenizeSound();
        RemoveSelf();
        turnedGold = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collided)
            return;
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collided = true;
            if (GameManager.Instance.Zombies.CurrentFormID != Crocodile.SPELLCASTERID)
            {
                boxCollider.isTrigger = true;
                animator.SetTrigger("damage");
                GameplayMusicManager.Instance.PlayBoomSound();
                GameManager.Instance.CallExplosion(true, transform.position);
            }
        }
        else if (!collision.gameObject.CompareTag("Road"))
            RemoveSelf();
    }
}
