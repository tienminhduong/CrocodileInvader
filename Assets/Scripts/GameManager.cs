using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);
        else
            _instance = this;
    }
    #endregion

    [SerializeField] private List<Manager> managers = new List<Manager>();
    [SerializeField] private Transform spawnerPosition;
    [SerializeField] private Transform detector1, detector2;

    [SerializeField] private float scrollBackSpeed;
    [SerializeField] private float deltaSpawnTime;

    [Header("Bonus block")]
    [SerializeField] private BonusBlock bonusBlock;
    [SerializeField] private float bonusBlockMaxTime;
    [SerializeField] private float bonusBlockCountTime;
    [SerializeField] private float bonusBlockActiveMaxTime;
    [SerializeField] private float bonusBlockActiveCountTime;
    private bool isCountSpawnBlock = true;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI zombieNumberText;
    [SerializeField] private TextMeshProUGUI coinNumberText;
    [SerializeField] private TextMeshProUGUI brainNumberText;
    [SerializeField] private GameObject pausedUI;

    [Header("Debug only")]
    [SerializeField] private int spawnOnly;


    private int coinNumber;
    private int brainNumber;
    [SerializeField] private int initialZombieNumber;

    private float spawnTimeCount;
    [SerializeField] private int spawnCode = 0; // 0: Normal, 1: Bonus block, 2: Two column coin

    #region Properties
    public float ScrollBackSpeed => scrollBackSpeed + Mathf.Min(brainNumber / 5, 15);
    public int CoinNumber => coinNumber;
    public int BrainNumber => brainNumber;
    public Vector3 SpawnerPosition => spawnerPosition.position;
    static public float ScreenHeight => 2f * Camera.main.orthographicSize;
    static public float ScreenWidth => ScreenHeight * Camera.main.aspect;
    public bool CanSpawn
    {
        get
        {
            RaycastHit2D ht1 = Physics2D.Raycast(detector1.position, Vector2.down);
            RaycastHit2D ht2 = Physics2D.Raycast(detector2.position, Vector2.down);

            if (!ht1 || !ht2)
                return false;
            return ht1.collider.gameObject == ht2.collider.gameObject;
        }
    }

    public ZombieManager Zombies => (ZombieManager)managers[0];
    public CoinManager Coins => (CoinManager)managers[4];
    #endregion Properties


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ScreenSize: " + ScreenWidth + "x" + ScreenHeight);

        brainNumber = 0;
        GenerateZombies(initialZombieNumber);
        spawnTimeCount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBonusBlockUpdate();
        SpawnItem();
        UpdateText();
    }

    private void SpawnItem()
    {
        spawnTimeCount += Time.deltaTime;
        if (spawnTimeCount >= deltaSpawnTime && CanSpawn)
        {
            if (spawnCode == 0)
            {
                if (spawnOnly == -1)
                    managers[Random.Range(2, managers.Count)].CallSpawnItem();
                else
                    managers[spawnOnly].CallSpawnItem();
            }
            else
            {
                if (spawnCode == 1)
                    SetActiveBonusBlock();
                else
                    Coins.GetItem(2);
                spawnCode = 0;
            }
            spawnTimeCount = 0f;
        }
    }

    private void UpdateText()
    {
        zombieNumberText.text = "x" + Zombies.Count.ToString();
        coinNumberText.text = coinNumber.ToString();
        brainNumberText.text = brainNumber.ToString();
    }

    public void GenerateZombies(int number = 1)
    {
        for (int i = 0; i < number; i++)
            Zombies.AddZombie();
        brainNumber += number;
    }

    public void IncCoin() { coinNumber++; }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausedUI.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        pausedUI.SetActive(false);
    }

    private void CheckBonusBlockUpdate()
    {
        //if (bonusBlockActiveCountTime > 0f)
        //{
        //    bonusBlockActiveCountTime -= Time.deltaTime;
        //    if (bonusBlockActiveCountTime <= 0f)
        //    {
        //        //call 2 column coins
        //        Instance.Coins.GetItem(2);

        //    }
        //}
        //if (isCountSpawnBlock)
        //    bonusBlockCountTime += Time.deltaTime;
        //if (bonusBlockCountTime >= bonusBlockMaxTime)
        //{
        //    RaycastHit2D hit = Physics2D.Raycast((Vector2)SpawnerPosition + Vector2.up * 10f, Vector2.down);
        //    if (hit.collider && hit.collider.gameObject.CompareTag("Road"))
        //    {
        //        bonusBlockCountTime = 0f;
        //        SetActiveBonusBlock();
        //    }
        //}
        if (isCountSpawnBlock)
            bonusBlockCountTime += Time.deltaTime;
        if (bonusBlockCountTime >= bonusBlockMaxTime)
        {
            spawnCode = 1;
            isCountSpawnBlock = false;
            bonusBlockCountTime = 0f;
        }
        if (bonusBlockActiveCountTime > 0f)
        {
            bonusBlockActiveCountTime -= Time.deltaTime;
            if (bonusBlockActiveCountTime <= 0f)
                spawnCode = 2;
        }
    }

    private void SetActiveBonusBlock()
    {
        bonusBlock.gameObject.SetActive(true);
        bonusBlock.transform.position = new Vector3(SpawnerPosition.x, bonusBlock.transform.position.y, bonusBlock.transform.position.z);
        isCountSpawnBlock = false;
    }

    public void ActivateTransform()
    {
        Instance.Zombies.ChangeForm(1);
        bonusBlock.gameObject.SetActive(false);
        bonusBlockActiveCountTime = bonusBlockActiveMaxTime;
    }

    public void DeactivateTranform()
    {
        Instance.Zombies.ChangeForm(0);
        isCountSpawnBlock = true;
    }
}
