using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBlock : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;
    private float Width => boxCollider.size.x;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        transform.position += Vector3.left * GameManager.Instance.ScrollBackSpeed * Time.deltaTime;
        if (transform.position.x + Width / 2 < GameManager.ScreenWidth * -0.55f)
            gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
            GameManager.Instance.ActivateTransform();
    }
}
