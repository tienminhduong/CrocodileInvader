using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SpriteRenderer spriteRenderer;
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

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
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
        if (spriteRenderer.enabled && collision.gameObject.CompareTag("Zombie"))
        {
            if (audioSource)
                audioSource.Play();
            spriteRenderer.enabled = false;
            GameManager.Instance.IncCoin();
            
        }
    }
}
