using System.Collections;
using System.Collections.Generic;
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


    private int coinNumber;
    private int brainNumber;
    private int initialZombieNumber;

    private float t;

    #region Properties
    public float ScrollBackSpeed => scrollBackSpeed;
    public int CoinNumber => coinNumber;
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
    public RoadManager Roads => (RoadManager)managers[1];
    public HumanManager Humans => (HumanManager)managers[2];
    #endregion Properties


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ScreenSize: " + ScreenWidth + "x" + ScreenHeight);

        initialZombieNumber = 3;
        brainNumber = initialZombieNumber;
        GenerateZombies(initialZombieNumber);
        t = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t >= deltaSpawnTime && CanSpawn)
        {
            managers[Random.Range(2, managers.Count)].CallSpawnItem();
            t = 0f;
        }
    }

    public void GenerateZombies(int number = 1)
    {
        for (int i = 0; i < number; i++)
            Zombies.AddZombie();
        brainNumber += number;
    }
}
