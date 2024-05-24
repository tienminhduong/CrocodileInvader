using System;
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
    [SerializeField] private float scrollBackSpeed;

    private int coinNumber;
    private int brainNumber;
    private int initialZombieNumber;

    #region Properties
    public float ScrollBackSpeed => scrollBackSpeed;
    public int CoinNumber => coinNumber;
    public Vector3 SpawnerPosition => spawnerPosition.position;
    static public float ScreenHeight => 2f * Camera.main.orthographicSize;
    static public float ScreenWidth => ScreenHeight * Camera.main.aspect;


    public float t = 0f;


    public ZombieManager Zombies => (ZombieManager)managers[0];
    public RoadManager Roads => (RoadManager)managers[1];
    #endregion Properties


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ScreenSize: " + ScreenWidth + "x" + ScreenHeight);

        initialZombieNumber = 3;
        brainNumber = initialZombieNumber;
        GenerateZombies(initialZombieNumber);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t >= 2f)
        {
            t = 0f;
            GenerateZombies(1);
        }
    }

    public void GenerateZombies(int number)
    {
        for (int i = 0; i < number; i++)
            Zombies.AddZombie();
        brainNumber += number;
    }
}
