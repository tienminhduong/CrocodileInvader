using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Zombie : PoolableObject
{
    [Header("Zombie")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    [SerializeField] private List<Collision2D> collisions = new List<Collision2D>();

    private float waitForJump;
    private float waitForFall;
    private bool isTouchingScreen;
    [SerializeField] private float groundHeight;
    [Header("Jump stats")]
    /// <summary>
    /// -1: Falling, 0: On ground, 1: Jumping, 2: Floating
    /// </summary>
    [SerializeField] private int jumpStatus;
    [SerializeField] private float jumpSpeed;

    private float jumpAcceleration;
    private float maxJumpAcceleration;

    protected float maxJumpHeight;
    private float CurrentHeight => transform.position.y - groundHeight;

    private float dForward;
    private float tForward;

    [SerializeField] private float floatingGravityScale;
    [SerializeField] private float fallingGravityScale;


    public override float Width => boxCollider.size.x;
    public override float Height => boxCollider.size.y;
    public int JumpStatus => jumpStatus;
    public int CollisionNumber => collisions.Count;
    public bool IsTouchingScreen => isTouchingScreen;
    public bool IsFellOffTheGround
    {
        get
        {
            return transform.position.y < groundHeight || (transform.position.y == groundHeight && jumpStatus == -1);
        }
    }

    private const int ZombieLayer0 = 5;
    private int currentLayer;
    public int Layer => currentLayer;

    // Start is called before the first frame update
    protected override void Start()
    {
        Init();

        //maxJumpHeight = boxCollider.size.y * 2f;
        //maxJumpAcceleration = -jumpSpeed * jumpSpeed / (2 * maxJumpHeight);
        SetJumpHeight(1f);
    }

    protected void SetJumpHeight(float modifier)
    {
        maxJumpHeight = boxCollider.size.y * 2f * modifier;
        maxJumpAcceleration = -jumpSpeed * jumpSpeed / (2 * maxJumpHeight);
    }

    public override void Init()
    {
        waitForFall = 0f;
        waitForJump = 0f;
        jumpStatus = -1;

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
    }

    public void SetLayer(int layer)
    {
        currentLayer = layer;
        foreach (SpriteRenderer renderer in renderers)
            renderer.sortingLayerName = "Layer" + layer.ToString();

        gameObject.layer = layer + ZombieLayer0;
    }

    public void CallTriggerJump(float time) { waitForJump = waitForJump > 0 ? waitForJump : time; }
    public void CallTriggerFall(float time) { waitForFall = waitForFall > 0 ? waitForFall : time; }
    private void CheckJump()
    {
        if (waitForJump > 0)
        {
            waitForJump -= Time.deltaTime;
            if (waitForJump < 0 && jumpStatus == 0)
            {
                isTouchingScreen = true;
                jumpStatus = 1;
                jumpAcceleration = maxJumpAcceleration;

                dForward = GameManager.ScreenWidth / 20f;
                if (Random.Range(0, 100) < 10 && GameManager.Instance.Zombies.FirstZombie)
                    dForward += (GameManager.Instance.Zombies.FirstZombie.transform.position.x - transform.position.x) * 0.5f;
                if (Random.Range(0, 100) < 10 || transform.position.x + dForward >= -0.1f * GameManager.ScreenWidth)
                    dForward = 0f;
            }
        }
    }
    private void CheckFall()
    {
        if (waitForFall > 0)
        {
            waitForFall -= Time.deltaTime;
            if (waitForFall < 0)
                isTouchingScreen = false;
        }
    }

    private void UpdateJumpFall()
    {
        CheckJump();
        CheckFall();

        if (jumpStatus == 1)
            rigidBody.gravityScale = 0f;
        else if (jumpStatus == 2)
            rigidBody.gravityScale = floatingGravityScale;
        else
            rigidBody.gravityScale = fallingGravityScale;

        if (jumpStatus == 1)
        {
            float v = Mathf.Sqrt(Mathf.Max(2 * jumpAcceleration * CurrentHeight + jumpSpeed * jumpSpeed, 0f));
            transform.position += Vector3.up * v * Time.deltaTime;

            if (CurrentHeight >= maxJumpHeight * 0.75f && isTouchingScreen == false)
                jumpStatus = -1;
            if (CurrentHeight >= maxJumpHeight)
                jumpStatus = 2;

            tForward += Time.deltaTime;
            float vF = 2 * dForward * (1 - tForward);
            if (vF < 0f)
                dForward = tForward = 0f;
            else
            {
                float d = vF * Time.deltaTime;
                transform.position += d * Vector3.right;
                if (waitForFall > 0.001f)
                {
                    waitForFall -= d / GameManager.Instance.ScrollBackSpeed;
                    if (waitForFall <= 0f)
                        waitForFall = 0.001f;
                }
            }
        }
        if (jumpStatus == 2 && isTouchingScreen == false)
            jumpStatus = -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Road") && transform.position.y > collision.gameObject.transform.position.y)
        {
            jumpStatus = 0;
            groundHeight = collision.transform.position.y;
            isTouchingScreen = false;
        }
        else if (collision.gameObject.CompareTag("Object"))
        {
            if (ID != 1 && !collisions.Contains(collision))
                collisions.Add(collision);
        }

        OnCollisionEnterObjectBehavior(collision);
    }

    protected virtual void OnCollisionEnterObjectBehavior(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
            RemoveSelf();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Road") && rigidBody.velocity.y < 0)
            jumpStatus = -1;
        if (collision.gameObject.CompareTag("Object") && collisions.Contains(collision))
            collisions.Remove(collision);
    }
}
