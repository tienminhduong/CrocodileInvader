using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : PoolableObject
{
    [SerializeField] private BoxCollider2D boxCollider;
    private bool turnedGold = false;

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
        if (!turnedGold && GameManager.Instance.Zombies.FirstZombie &&
            (transform.position - GameManager.Instance.Zombies.FirstZombie.transform.position).magnitude
            <= GameManager.ScreenWidth * 0.2f &&
            GameManager.Instance.Zombies.CurrentFormID == Zombie.SPELLCASTERID)
            TurnIntoGold();
    }

    public override void Init()
    {
        base.Init();
        turnedGold = false;
    }

    private void TurnIntoGold()
    {
        GameManager.Instance.Coins.TranformIntoCoin(this, true, 0);
        GameManager.Instance.Zombies.FirstZombie.PlayAttackAnimation();
        GameplayMusicManager.Instance.PlayGoldenizeSound();
        RemoveSelf();
        turnedGold = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            RemoveSelf();
            if (GameManager.Instance.Zombies.CurrentFormID == Zombie.SPELLCASTERID)
            {
            }
            else
            {
                GameplayMusicManager.Instance.PlayBoomSound();
            }
        }
        else if (!collision.gameObject.CompareTag("Road"))
            RemoveSelf();
    }
}
