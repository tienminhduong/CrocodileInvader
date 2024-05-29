using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Zombies.CurrentFormID == 1)
            MoveToZombie();
    }

    private void MoveToZombie()
    {
        Zombie target = GameManager.Instance.Zombies.FirstZombie;
        if (!target)
            return;
        Vector3 d = transform.position - target.transform.position;
        if (d.magnitude <= GameManager.ScreenWidth * 0.15f)
            transform.position -= 10f * Time.deltaTime * d;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            gameObject.SetActive(false);
            GameManager.Instance.IncCoin();
        }
    }
}
