using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZombieManager : Manager
{
    public override PoolableObject GetItem(int id = 0)
    {
        Zombie z = (Zombie)base.GetItem(id);
        z.transform.position = transform.position;

        return z;
    }

    [SerializeField] private List<Zombie> zombieList = new List<Zombie>();
    [SerializeField] private List<float> associatedX;
    [SerializeField] private int currentZombieID;
    private int layerCount = 1;
    private Zombie firstZombie;

    #region Properties
    public Zombie FirstZombie => firstZombie;

    public int Count => zombieList.Count;
    public int CurrentFormID => currentZombieID;
    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CollectingNulls();
        LoadFirstZombie();
        ZombiesJumping();
        CalculatePosition();
        RearrangeZombies();
    }

    private void LoadFirstZombie()
    {
        if (zombieList.Count == 0) { firstZombie = null; return; }
        firstZombie = null;
        float maxX = -GameManager.ScreenWidth;
        foreach (Zombie zombie in zombieList)
            if (zombie.JumpStatus != -1 && zombie.transform.position.x > maxX)
            { firstZombie = zombie; maxX = zombie.transform.position.x; }
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
            if (zombie.JumpStatus != 0) return false;
        return true;
    }

    private bool AreAllNotTouchingAnythingOtherThanGround()
    {
        foreach (Zombie zombie in zombieList)
            if (zombie.CollisionNumber > 0) return false;
        return true;
    }

    private float GetDelayedTime(Zombie zombie)
    {
        float delayModifier = 1f;
        if (Count >= 10)
            delayModifier = 1f;
        return (FirstZombie.transform.position.x - zombie.transform.position.x) / GameManager.Instance.ScrollBackSpeed * delayModifier + 0.001f;
    }

    private void ZombiesJumping()
    {
        if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (FirstZombie && FirstZombie.JumpStatus == 0)
                {
                    foreach (Zombie zombie in zombieList)
                        zombie.CallTriggerJump(GetDelayedTime(zombie));
                    GameplayMusicManager.Instance.PlayJumpSound();
                }
            }
            if (touch.phase == TouchPhase.Ended && FirstZombie)
                foreach (Zombie zombie in zombieList)
                    zombie.CallTriggerFall(GetDelayedTime(zombie));
        }
    }

    private void CalculatePosition()
    {
        if (!FirstZombie || !AreAllOnGround()
            || !AreAllNotTouchingAnythingOtherThanGround()) return;
        associatedX = new List<float>();

        float availableWidth = 0.3f * GameManager.ScreenWidth;
        if (Count > 20)
            availableWidth = 0.4f * GameManager.ScreenWidth;
        float lastPossibleXPosition = -0.4f * GameManager.ScreenWidth;

        float d = availableWidth / zombieList.Count;
        float maxD = 0.75f * FirstZombie.Width;
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
                zombieList[i].transform.position += 2 * d * Time.deltaTime * Vector3.left;
            }
        }
    }

    public void ChangeForm(int id)
    {
        for (int i = 0; i < Count; ++i)
        {
            Zombie temp = zombieList[i];
            zombieList[i] = (Zombie)GetItem(id);
            zombieList[i].transform.position = temp.transform.position;
            zombieList[i].SetLayer(temp.Layer);
            ReturnItem(temp);
        }
        currentZombieID = id;
    }
}
