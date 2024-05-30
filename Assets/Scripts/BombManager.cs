using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : Manager
{
    [SerializeField] private int continuousBombRate;
    [SerializeField] private int multipleBombRate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CallSpawnItem()
    {
        if (GameManager.Instance.BrainNumber >= 15 && Random.Range(0, 100) < multipleBombRate)
            SpawnMultipleBomb();
        else if (GameManager.Instance.Zombies.Count >= 4 && GameManager.Instance.ScrollBackSpeed >= 10
            && Random.Range(0, 100) < continuousBombRate)
            SpawnContinuousBomb();
        else
            base.CallSpawnItem();
    }

    private void SpawnMultipleBomb()
    {
        if (!GameManager.Instance.Zombies.FirstZombie)
            return;
        Vector3 position = GameManager.Instance.SpawnerPosition + Vector3.left * 10f;
        Bomb bomb = (Bomb)GetItem();
        float width = bomb.Width + 1f, zombieWidth = GameManager.Instance.Zombies.FirstZombie.Width * 2.5f;
        position += Vector3.right * width;
        bomb.transform.position = position;
        position += Vector3.right * (width + zombieWidth);
        for (int i = 1; i < 4; ++i)
        {
            Bomb b = (Bomb)GetItem();
            b.transform.position = position;
            position += Vector3.right * (width + zombieWidth);
        }
    }
    private void SpawnContinuousBomb()
    {
        Vector3 origin = GameManager.Instance.SpawnerPosition + Vector3.left * 4f;
        Bomb bomb = (Bomb)GetItem();
        float width = bomb.Width + 1f;
        bomb.transform.position = origin + Vector3.right * width;
        for (int i = 1; i < 4; ++i)
        {
            Bomb b = (Bomb)GetItem();
            b.transform.position = origin + Vector3.right * (i + 1) * width;
        }
    }
}
