using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Box : PoolableObject
{
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private int numberHumansContains;
    [SerializeField] private int numberCrocodileNeeded;
    [SerializeField] private List<Collision2D> collisions = new List<Collision2D>();
    [SerializeField] private Slider slider;

    [SerializeField] private TextMeshProUGUI text;

    private bool turnedGold = false;

    public override void Init()
    {
        base.Init();
        collisions.Clear();
    }

    public override float Width => boxCollider2D.size.x;
    public override float Height => boxCollider2D.size.y;
    public int CollisionCount => collisions.Count;

    // Start is called before the first frame update
    protected override void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (CollisionCount >= numberCrocodileNeeded)
        {
            GeneratePreys();
            GameplayMusicManager.Instance.PlayBoomSound();
            GameManager.Instance.CallExplosion(false);
            RemoveSelf();
        }
        text.text = CollisionCount.ToString() + "/" + numberCrocodileNeeded.ToString();
        slider.value = (float)CollisionCount / numberCrocodileNeeded;

        if (!turnedGold && GameManager.Instance.Zombies.FirstZombie &&
            (transform.position - GameManager.Instance.Zombies.FirstZombie.transform.position).magnitude
            <= GameManager.ScreenWidth * 0.2f &&
            GameManager.Instance.Zombies.CurrentFormID == Crocodile.SPELLCASTERID)
            TurnIntoGold();
    }

    private void TurnIntoGold()
    {
        GameManager.Instance.Coins.TranformIntoCoin(this, false, ID);
        GameManager.Instance.Zombies.FirstZombie.PlayAttackAnimation();
        GameplayMusicManager.Instance.PlayGoldenizeSound();
        GeneratePreys();
        RemoveSelf();
        turnedGold = true;
    }

    private void GeneratePreys()
    {
        for (int i = 0; i < numberHumansContains; ++i)
        {
            Chicken h = (Chicken)GameManager.Instance.Humans.GetItem(Random.Range(0,
                GameManager.Instance.Humans.PrefabsCount));
            h.transform.position = gameObject.transform.position + Vector3.right * Width;
            h.SetLayer(Random.Range(1, 4));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            if (GameManager.Instance.Zombies.CurrentFormID == 0)
            {
                collisions.Add(collision);
            }
            else if (GameManager.Instance.Zombies.CurrentFormID == 1)
            {
                //Call coin manager generate coin
                GameManager.Instance.Coins.TranformIntoCoin(this, false, ID);
                RemoveSelf();
                GameManager.Instance.GenerateZombies(numberHumansContains);
                GameplayMusicManager.Instance.PlayGoldenizeSound();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collisions.Contains(collision))
            collisions.Remove(collision);
    }

    protected override void DestroyOnOutOfBounds()
    {
        if (transform.position.x + Width < GameManager.ScreenWidth * -0.55f)
            RemoveSelf();
        if (transform.position.y + Height < GameManager.ScreenHeight / -2)
            RemoveSelf();
    }
}