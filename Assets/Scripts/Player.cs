using System.Collections;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : Singleton<Player>
{
    [SerializeField] float movementSpeed = 5;

    [Header("Jump settings")]
    [Space(10)]
    [SerializeField] float jumpForce = 5;
    [SerializeField] float isGroundedCheckRange = 0.5f;
    [SerializeField] LayerMask groundLayer = 0;
    [SerializeField] AudioSource jumpSound = null;

    [Header("Dash settings")]
    [Space(10)]
    [SerializeField] Vector2 dashForce = new Vector2();
    [SerializeField] float dashCooldown = 2;
    [SerializeField] float dashTime = 1;
    [SerializeField] ParticleSystem dashEffect = null;
    [SerializeField] AudioSource dashSound = null;
    float lastDashTime = float.MinValue;
    bool dashedInAir = false;

    bool canMove = true;
    bool isGrounded = false;
    bool isJumping = false;

    Animator anim = null;
    Collider2D col = null;
    SpriteRenderer sr = null;

    [Header("Checkpoints & Camera")]
    [Space(10)]
    [SerializeField]Checkpoint lastCheckpoint = null;
    [SerializeField]
    CinemachineVirtualCamera virtualCam = null;
    CinemachineFramingTransposer transposer = null;
    Camera cam = null;
    [SerializeField] float buffer = 0.01f;
    [SerializeField] float resetTime = 0.5f;
    [SerializeField] float deadZoneWidth = 0.5f;
    public Checkpoint LastCheckpoint { get { return lastCheckpoint; } private set { lastCheckpoint = value; } }

    public Rigidbody2D RB2D { get; protected set; }

    void Respawn()
    {
        transform.position = lastCheckpoint.transform.position;
        StartCoroutine(ResetCamera());
        col.enabled = true;
        anim.SetBool("Dead", false);
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsJumping", false);
    }

    IEnumerator ResetCamera()
    {
        transposer.m_DeadZoneWidth = 0.3f;
        yield return new WaitForSeconds(resetTime);
        transposer.m_DeadZoneWidth = deadZoneWidth;
        canMove = true;
    }



    protected override void Awake()
    {
        base.Awake();
        if (lastCheckpoint != null)
            lastCheckpoint.Triggered = true;

        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        transposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();

        RB2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameManager.Instance.onDeath += Respawn;
    }

    void Update()
    {
        isGrounded = IsGrounded();

        MoveHorizontally();
        isJumping = Jump();
        Dash();

        if (RB2D.velocity.x < 0 && CantMoveLeft(buffer))
        {
            RB2D.velocity = new Vector2(0, RB2D.velocity.y);
        }

        anim.SetBool("IsJumping", !isGrounded);

        bool IsGrounded()
        {
            Vector3 size = col.bounds.size;
            size.x *= 0.9f;
            return Physics2D.BoxCast(transform.position, size, 0, Vector2.down, isGroundedCheckRange, groundLayer);
        }
    }

    public void SetNewCheckpoint(Checkpoint checkpoint)
    {
        KeyManager.Instance.SaveProgress();
        LastCheckpoint = checkpoint;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ActivateTooltip tooltip = other.GetComponent<ActivateTooltip>();
        if (tooltip && tooltip.ToolTip)
            tooltip.SetTooltip(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ActivateTooltip tooltip = other.GetComponent<ActivateTooltip>();
        if (tooltip && tooltip.ToolTip)
            tooltip.SetTooltip(false);
    }

    void MoveHorizontally()
    {
        if (!canMove)
            return;

        float horizontal = Input.GetAxis("Horizontal");

        Vector2 velocity = RB2D.velocity;
        velocity.x = horizontal * movementSpeed;
        RB2D.velocity = velocity;

        if (horizontal != 0)
            sr.flipX = horizontal < 0;
        anim.SetBool("IsWalking", horizontal != 0);
    }

    bool CantMoveLeft(float buffer)
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(transform.position);
        return screenPoint.x < buffer;
    }

    bool Jump()
    {
        if (!canMove)
            return false;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Vector2 velocity = RB2D.velocity;
            velocity.y = jumpForce;
            RB2D.velocity = velocity;
            jumpSound.Play();
            return true;
        }
        return false;
    }

    void Dash()
    {
        if (isGrounded)
            dashedInAir = false;

        if (!canMove)
            return;

        if (!isJumping && Input.GetButtonDown("Jump") && CanDash())
        {
            StartCoroutine(DashCoroutine());

            if (sr.flipX)
                dashEffect.transform.localRotation = Quaternion.LookRotation(Vector3.right);
            else
                dashEffect.transform.localRotation = Quaternion.LookRotation(Vector3.left);
            dashEffect.Play();
            dashSound.Play();

            lastDashTime = Time.time;
            if (!isGrounded)
                dashedInAir = true;
        }

        bool CanDash()
        {
            return lastDashTime + dashCooldown <= Time.time && (isGrounded || !dashedInAir);
        }

        IEnumerator DashCoroutine()
        {
            canMove = false;

            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (direction == Vector2.zero)
                direction = Vector2.up;
            direction.x *= dashForce.x;
            direction.y *= dashForce.y;

            float waitedTime = 0;
            while (waitedTime < dashTime)
            {
                if (isGrounded)
                    break;
                if (RB2D.velocity.y < 0)
                {
                    Vector2 velocity = RB2D.velocity;
                    velocity.y = 0;
                    RB2D.velocity = velocity;
                }
                RB2D.velocity = direction;
                waitedTime += Time.deltaTime;
                yield return null;
            }

            canMove = true;
        }
    }

    public void Die()
    {
        canMove = false;
        col.enabled = false;
        RB2D.velocity = new Vector2(0, 5);
        anim.SetBool("Dead", true);
        if (GameManager.Instance)
            GameManager.Instance.Lose(this);
    }

    void OnDrawGizmosSelected()
    {
        if (col)
        {
            Vector3 size = col.bounds.size;
            size.x *= 0.9f;
            Gizmos.DrawCube((Vector2)transform.position + Vector2.down * isGroundedCheckRange, size);
        }    
    }
}
