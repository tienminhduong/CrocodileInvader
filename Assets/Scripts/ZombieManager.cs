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
        z.Init();

        return z;
    }

    [SerializeField] private List<Zombie> zombieList = new List<Zombie>();
    [SerializeField] private List<float> associatedX;
    private int currentZombieID = 0;
    private int layerCount = 1;

    #region Properties
    //public Zombie FirstZombie { get { return zombieList.Count > 0 ? zombieList[0] : null; } }
    public Zombie FirstZombie
    {
        get
        {
            if (zombieList.Count == 0) return null;
            Zombie result = null;
            float maxX = -GameManager.ScreenWidth;
            foreach (Zombie zombie in zombieList)
                if (!zombie.IsFellOffTheGround && zombie.transform.position.x > maxX)
                { result = zombie; maxX = zombie.transform.position.x; }
            return result;
        }
    }
    public Zombie LastZombie { get { return zombieList.Count > 0 ? zombieList[^1] : null; } }
    #endregion Properties

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        CollectingNulls();
        ZombiesJumping();
        CalculatePosition();
        RearrangeZombies();
    }

    private void CollectingNulls()
    {
        for (int i = 0; i < zombieList.Count; i++)
            if (!zombieList[i].isActiveAndEnabled) { zombieList.RemoveAt(i); i--; }
    }

    public void AddZombie()
    {
        Zombie z = (Zombie)GetItem(currentZombieID);
        z.SetLayer(layerCount);

        if (layerCount == 1)
            layerCount = 3;
        else if (layerCount == 3)
            layerCount = 2;
        else
            layerCount = 1;

        zombieList.Add(z);
    }

    private bool AreAllOnGround()
    {
        foreach (Zombie zombie in zombieList)
            if (zombie.IsOnGround != 1) return false;
        return true;
    }

    private bool AreAllNotTouchingAnythingOtherThanGround()
    {
        foreach (Zombie zombie in zombieList)
            if (zombie.CollisionNumber > 0) return false;
        return true;
    }

    private void ZombiesJumping()
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

    private void CalculatePosition()
    {
        if (!FirstZombie || !AreAllOnGround()
            || !AreAllNotTouchingAnythingOtherThanGround()) return;
        associatedX = new List<float>();

        float availableWidth = 0.3f * GameManager.ScreenWidth;
        float lastPossibleXPosition = -0.4f * GameManager.ScreenWidth;

        float d = availableWidth / zombieList.Count;
        float maxD = 0.85f * FirstZombie.Width;
        if (maxD < d) d = maxD;

        for (int i = 0; i < zombieList.Count; i++)
        {
            float distance = (zombieList.Count - 1 - i) * d;
            associatedX.Add(lastPossibleXPosition + distance);
        }
    }

    private void RearrangeZombies()
    {
        if (associatedX == null || associatedX.Count < zombieList.Count) return;
        if (AreAllOnGround() && AreAllNotTouchingAnythingOtherThanGround())
        {
            for (int i = 0; i < zombieList.Count; ++i)
            {
                if (zombieList[i].transform.position.x == associatedX[i])
                    continue;
                float d = zombieList[i].transform.position.x - associatedX[i];
                zombieList[i].transform.position += Vector3.left * d * Time.deltaTime;
            }
        }
    }
}
