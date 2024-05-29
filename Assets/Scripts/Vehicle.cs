using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vehicle : PoolableObject
{
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private int numberHumansContains;
    [SerializeField] private int numberHumanNeeded;
    [SerializeField] private List<Collision2D> collisions = new List<Collision2D>();

    [SerializeField] private TextMeshProUGUI text;

    public int numberCollision;
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
        numberCollision = CollisionCount;
        if (CollisionCount >= numberHumanNeeded)
        {
            RemoveSelf();
            GameManager.Instance.GenerateZombies(numberHumansContains);
        }
        text.text = CollisionCount.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            if (GameManager.Instance.Zombies.CurrentFormID == 0)
                collisions.Add(collision);
            else if (GameManager.Instance.Zombies.CurrentFormID == 1)
            {
                //Call coin manager generate coin
                RemoveSelf();
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