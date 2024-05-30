using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinContainer : PoolableObject
{
    [SerializeField] private BoxCollider2D boxCollider;
    private List<Vector3> coinPosition;

    public override float Width => boxCollider.size.x;
    public override float Height => boxCollider.size.y;
    public override void Init()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            if (coinPosition == null)
            {
                coinPosition = new List<Vector3>();
                for (int j = 0; j < transform.childCount; ++j)
                    coinPosition.Add(transform.GetChild(j).position);
            }
            transform.GetChild(i).position = coinPosition[i];
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (ID == 2 && transform.position.x < -GameManager.ScreenWidth / 2f)
            GameManager.Instance.DeactivateTranform();
    }
}
