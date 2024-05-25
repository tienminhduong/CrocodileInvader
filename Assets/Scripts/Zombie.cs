using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Zombie : PoolableObject
{
    [Header("Zombie")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    [SerializeField] private List<Collision2D> collisions = new List<Collision2D>();
    [SerializeField] private Animator legAnimator;

    private float waitForJump;
    private float waitForFall;
    private float maxJumpHeight;
    private float groundHeight;

    private float dForward;
    private float tForward;

    [SerializeField] private float onGroundGravityScale;
    [SerializeField] private float fallingGravityScale;

    [SerializeField] private int isOnGround;

    public override float Width => boxCollider.size.x;
    public override float Height => boxCollider.size.y;
    public int IsOnGround => isOnGround; // 0: Jumping, -1: Falling, 1: On ground
    public int CollisionNumber => collisions.Count;
    public bool IsFellOffTheGround => transform.position.y < groundHeight;

    private const int ZombieLayer0 = 5;

    // Start is called before the first frame update
    protected override void Start()
    {
        Init();

        maxJumpHeight = boxCollider.size.y * 2f;
    }

    public override void Init()
    {
        waitForFall = 0f;
        waitForJump = 0f;
        isOnGround = -1;
        dForward = 0f;
        tForward = 0f;
        groundHeight = float.MaxValue;
        collisions.Clear();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        UpdateJumpFall();
        legAnimator.SetBool("isOnGround", isOnGround == 1);
    }

    public void SetLayer(int layer)
    {
        foreach (SpriteRenderer renderer in renderers)
            renderer.sortingLayerName = "Layer" + layer.ToString();

        gameObject.layer = layer + ZombieLayer0;
    }

    public void CallTriggerJump(float time) { waitForJump = waitForJump > 0 ? waitForJump : time; }
    public void CallTriggerFall(float time) { waitForFall = waitForFall > 0 ? waitForFall : time; }

    private void UpdateJumpFall()
    {
        if (waitForJump > 0)
        {
            waitForJump -= Time.deltaTime;
            if (waitForJump < 0)
                Jump();
        }
        if (waitForFall > 0)
        {
            waitForFall -= Time.deltaTime;
            if (waitForFall < 0)
                Fall();
        }

        if (isOnGround == 0 && dForward > 0)
        {
            tForward += Time.deltaTime;
            float v = 2 * dForward * (1 - tForward);
            if (v < 0f)
                dForward = tForward = 0f;
            else
                rigidBody.velocity = new Vector2(v, rigidBody.velocity.y);
        }

        if (isOnGround == 0 && rigidBody.velocity.y <= 0)
            rigidBody.gravityScale = fallingGravityScale;
        else
            rigidBody.gravityScale = onGroundGravityScale;
    }

    private void Jump()
    {
        rigidBody.velocity = Vector3.zero;
        float jumpForce = Mathf.Sqrt(maxJumpHeight * Physics2D.gravity.y * rigidBody.gravityScale * (-2))
            * rigidBody.mass;

        dForward = GameManager.ScreenWidth / 20f;
        if (Random.Range(0, 100) < 10 && GameManager.Instance.Zombies.FirstZombie)
            dForward += (GameManager.Instance.Zombies.FirstZombie.transform.position.x - transform.position.x) * 0.5f;
        if (Random.Range(0, 100) < 30 || transform.position.x + dForward >= -0.1f * GameManager.ScreenWidth)
            dForward = 0f;

        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isOnGround = 0;
    }

    private void Fall()
    {
        isOnGround = -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            isOnGround = 1;
            groundHeight = collision.transform.position.y;
            rigidBody.gravityScale = onGroundGravityScale;
        }
        else if (collision.gameObject.CompareTag("Object"))
        {
            if (!collisions.Contains(collision))
                collisions.Add(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Road") && rigidBody.velocity.y < 0)
            isOnGround = -1;
        if (collision.gameObject.CompareTag("Object") && collisions.Contains(collision))
            collisions.Remove(collision);
    }
}
