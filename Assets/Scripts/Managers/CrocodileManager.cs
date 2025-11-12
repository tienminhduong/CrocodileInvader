using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrocodileManager : Manager
{
    public override PoolableObject GetItem(int id = 0)
    {
        Crocodile c = (Crocodile)base.GetItem(id);
        c.transform.position = transform.position;

        return c;
    }

    [SerializeField] private List<Crocodile> crocodileList = new List<Crocodile>();
    [SerializeField] private List<float> associatedX;
    [SerializeField] private int currentCrocodileID;
    private int layerCount = 1;
    private Crocodile firstCrocodile;

    #region Properties
    public Crocodile FirstCrocodile => firstCrocodile;

    public int Count => crocodileList.Count;
    public int CurrentFormID => currentCrocodileID;
    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CollectingNulls();
        LoadCrocodileZombie();
        CrocodilesJumping();
        CalculatePosition();
        RearrangeCrocodiles();
    }

    private void LoadCrocodileZombie()
    {
        if (crocodileList.Count == 0) { firstCrocodile = null; return; }
        firstCrocodile = null;
        float maxX = -GameManager.ScreenWidth;
        foreach (Crocodile zombie in crocodileList)
            if (zombie.transform.position.x > maxX && !zombie.IsOutGround)
            { firstCrocodile = zombie; maxX = zombie.transform.position.x; }
    }

    private void CollectingNulls()
    {
        for (int i = 0; i < crocodileList.Count; i++)
            if (!crocodileList[i].isActiveAndEnabled) { crocodileList.RemoveAt(i); i--; }

    }

    public void AddZombie(bool isEating = false)
    {
        Crocodile c = (Crocodile)GetItem(currentCrocodileID);
        c.SetLayer(layerCount);

        if (isEating && GameManager.Instance.Zombies.FirstCrocodile)
            c.transform.position = GameManager.Instance.Zombies.FirstCrocodile.transform.position
                + new Vector3(-1f, 1f, 0f);

        if (layerCount == 1)
            layerCount = 3;
        else if (layerCount == 3)
            layerCount = 2;
        else
            layerCount = 1;

        crocodileList.Add(c);
        GameplayMusicManager.Instance.PlayChickenIntoCrocodileSound();
    }

    private bool AreAllOnGround()
    {
        foreach (Crocodile crocodile in crocodileList)
            if (crocodile.JumpStatus != 0) return false;
        return true;
    }

    private bool AreAllNotTouchingAnythingOtherThanGround()
    {
        foreach (Crocodile crocodile in crocodileList)
            if (crocodile.CollisionNumber > 0) return false;
        return true;
    }

    private float GetDelayedTime(Crocodile crocodile)
    {
        float delayModifier = 0.8f;
        if (CurrentFormID == 1)
            delayModifier = 0.5f;
        float distance = Mathf.Max(FirstCrocodile.transform.position.x - crocodile.transform.position.x, 0f);
        return distance / GameManager.Instance.ScrollBackSpeed * delayModifier + 0.001f;
    }

    private void CrocodilesJumping()
    {
        if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (FirstCrocodile && FirstCrocodile.JumpStatus == 0)
                {
                    foreach (Crocodile crocodile in crocodileList)
                        if (!crocodile.IsOutGround)
                            crocodile.CallTriggerJump(GetDelayedTime(crocodile));

                    GameplayMusicManager.Instance.PlayJumpSound();
                }
            }
            if (touch.phase == TouchPhase.Ended && FirstCrocodile)
                foreach (Crocodile crocodile in crocodileList)
                    crocodile.CallTriggerFall(GetDelayedTime(crocodile));
        }
        if (Input.touchCount == 0 && FirstCrocodile)
            foreach (Crocodile crocodile in crocodileList)
                if (crocodile.IsTouchingScreen)
                    crocodile.CallTriggerFall(GetDelayedTime(crocodile));
    }

    private void CalculatePosition()
    {
        if (!FirstCrocodile || !AreAllOnGround()
            || !AreAllNotTouchingAnythingOtherThanGround()) return;
        associatedX = new List<float>();

        float availableWidth = 0.3f * GameManager.ScreenWidth;
        if (Count > 20)
            availableWidth = 0.4f * GameManager.ScreenWidth;
        float lastPossibleXPosition = -0.4f * GameManager.ScreenWidth;

        float d = availableWidth / crocodileList.Count;
        float maxD = 0.75f * FirstCrocodile.Width;
        if (maxD < d) d = maxD;

        for (int i = 0; i < crocodileList.Count; i++)
        {
            float distance = (crocodileList.Count - 1 - i) * d;
            associatedX.Add(lastPossibleXPosition + distance);
        }
    }

    private void RearrangeCrocodiles()
    {
        if (associatedX == null || associatedX.Count < crocodileList.Count) return;
        if (AreAllOnGround() && AreAllNotTouchingAnythingOtherThanGround())
        {
            for (int i = 0; i < crocodileList.Count; ++i)
            {
                if (crocodileList[i].transform.position.x == associatedX[i])
                    continue;
                float d = crocodileList[i].transform.position.x - associatedX[i];
                crocodileList[i].transform.position += 2 * d * Time.deltaTime * Vector3.left;
            }
        }
    }

    public void ChangeForm(int id)
    {
        if (id != 0)
            GameplayMusicManager.Instance.PlayGoldenizeSound();
        for (int i = 0; i < Count; ++i)
        {
            Crocodile temp = crocodileList[i];
            crocodileList[i] = (Crocodile)GetItem(id);
            crocodileList[i].transform.position = temp.transform.position + Vector3.up * 0.1f;
            crocodileList[i].SetLayer(temp.Layer);
            ReturnItem(temp);
        }
        currentCrocodileID = id;
    }
}
