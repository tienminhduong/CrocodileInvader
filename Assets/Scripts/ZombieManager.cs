using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : Manager
{
    public override PoolableObject GetItem(int id = 0)
    {
        Zombie z = (Zombie)base.GetItem(id);
        z.transform.position = transform.position;

        return z;
    }

    private List<Zombie> zombieList = new List<Zombie>();
    private int currentZombieID;

    #region Properties
    public Zombie FirstZombie { get { return zombieList.Count > 0 ? zombieList[0] : null; } }
    public Zombie LastZombie { get { return zombieList.Count > 0 ? zombieList[^1] : null; } }
    #endregion Properties

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentZombieID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (FirstZombie && FirstZombie.IsOnGround == 1)
                    foreach (Zombie zombie in zombieList)
                    {
                        float t = (FirstZombie.transform.position.x - zombie.transform.position.x)
                            / GameManager.Instance.ScrollBackSpeed;
                        zombie.CallTriggerJump(t + 0.01f);
                    }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (FirstZombie && FirstZombie.IsOnGround == 0)
                {
                    foreach (Zombie zombie in zombieList)
                    {
                        float t = (FirstZombie.transform.position.x - zombie.transform.position.x)
                            / GameManager.Instance.ScrollBackSpeed;
                        zombie.CallTriggerFall(t + 0.01f);
                    }
                }
            }
        }
    }

    public void AddZombie()
    {
        Zombie z = (Zombie)GetItem(currentZombieID);
        z.SetLayer(Random.Range(1, 4));
        zombieList.Add(z);
    }

    private bool AreAllOnGround()
    {
        foreach (Zombie zombie in zombieList)
            if (zombie.IsOnGround != 1) return false;
        return true;
    }

    private void RearrangeZombies()
    {
        if (AreAllOnGround())
        {
            
        }
    }
}
