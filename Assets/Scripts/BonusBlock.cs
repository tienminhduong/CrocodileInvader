using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBlock : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private GameObject GoldPopUp;

    [SerializeField] private GameObject topCover, bottomCover;

    private float countdownPopUp;
    //private bool isCountdownPopUp = false;
    private float Width => boxCollider.size.x;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countdownPopUp > 0)
        {
            countdownPopUp -= Time.unscaledDeltaTime;
            if (countdownPopUp <= 0)
            {
                Time.timeScale = 1f;
                GoldPopUp.SetActive(false);
                topCover.SetActive(false);
                bottomCover.SetActive(false);
                GameManager.Instance.ActivateTransform();
            }
        }

        transform.position += Vector3.left * GameManager.Instance.ScrollBackSpeed * Time.deltaTime;
        if (transform.position.x + Width / 2 < GameManager.ScreenWidth * -0.55f)
            GameManager.Instance.SetDeactivateBonusBlock();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            GoldPopUp.SetActive(true);
            topCover.SetActive(true);
            bottomCover.SetActive(true);
            GameplayMusicManager.Instance.PlayTransformSound();
            Time.timeScale = 0f;
            countdownPopUp = 2f;
        }
    }
}
