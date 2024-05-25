using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : Manager
{
    [SerializeField] private List<float> standardWidth = new List<float>();
    [SerializeField] private List<float> standardHeight = new List<float>();

    [Header("Watch only")]
    [SerializeField] private float countDistance;
    [SerializeField] private float standardDistance;
    [SerializeField] private float currentHeight;
    [SerializeField] private float nextHeight;
    [SerializeField] private int nextRoadID;

    //public float CurrentHeight => currentHeight;
    public float MinHeight => standardHeight[0];

    public override PoolableObject GetItem(int id = 0)
    {
        Road r = (Road)base.GetItem(id);

        int l = nextHeight == standardHeight[0] ? 0 : 1;
        r.Config(standardWidth[nextRoadID], l);

        r.transform.position = GameManager.Instance.SpawnerPosition;
        r.transform.position = new Vector3(r.transform.position.x + r.Width / 2,
            nextHeight, transform.position.z);


        return r;
    }

    // Start is called before the first frame update
    void Start()
    {
        countDistance = 0;
        currentHeight = standardHeight[0];
        nextHeight = currentHeight;
        nextRoadID = 2;
    }

    // Update is called once per frame
    void Update()
    {
        standardDistance = GameManager.Instance.Zombies.FirstZombie ?
            GameManager.Instance.Zombies.FirstZombie.Width * 2f : 0;

        countDistance -= GameManager.Instance.ScrollBackSpeed * Time.deltaTime;
        if (countDistance <= 0)
            SpawnRoad();
    }

    private void SpawnRoad()
    {
        Road r = (Road)GetItem();

        // Determine stat for next road
        float width = r.Width;
        float distance = standardDistance * GameManager.Instance.ScrollBackSpeed / 3f;
        nextHeight = standardHeight[Random.Range(0, standardHeight.Count)];

        int rd = Random.Range(0, 100);

        if (r.Width == standardWidth[0])
            rd = 0;

        float m1 = 0f, m2 = 1f;
        if (rd < 50)
        {
            // Spawn normally
            nextRoadID = Random.Range(0, standardWidth.Count);
            if (nextRoadID == 0 && r.Width == standardWidth[0])
                nextHeight = currentHeight;
        }
        else if (rd < 50 + 35)
        {
            // Spawn overlap road
            m1 = -standardWidth[0] * 0.5f;
            m2 = 0f;
            if (currentHeight == standardHeight[0])
                nextHeight = standardHeight[1];
            else nextHeight = standardHeight[0];

            nextRoadID = Random.Range(1, standardWidth.Count);
        }
        else
        {
            // Spawn right next road
            m2 = 0f;
        }

        if (nextHeight > currentHeight)
            m2 *= 0.5f;

        countDistance += (width + m1 + distance * m2);
        currentHeight = nextHeight;
    }
}
