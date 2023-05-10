using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField] protected Rigidbody2D _rigidbody;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected BoxCollider2D _boxCollider;

    [Header("Layer")]
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected LayerMask _wallLayer;

    [SerializeField] protected bool grounded;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float jumpPower = 10f;
    [SerializeField] protected float horizontal;
    [SerializeField] protected float wallJumpCooldown;
    [SerializeField] protected float coyoteCooldown = 0.25f;
    [SerializeField] protected float coyoteTimer;

    [Header("Multiple Jumps")]
    [SerializeField] protected int extraJumps = 2;
    [SerializeField] protected int jumpCounter;

    [Header("Walk Jump")]
    [SerializeField] protected float wallJumpX = 2000f;
    [SerializeField] protected float wallJumpY = 750f;

    [Header("Audio Jump")]
    [SerializeField] private AudioClip jumpSound;


    private void Awake()
    {
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._animator = GetComponent<Animator>();
        this._boxCollider = GetComponent<BoxCollider2D>();

    }

    private void Update()
    {
        this.horizontal = Input.GetAxis("Horizontal");

        // Flip player when moving left-right
        if (this.horizontal > 0.01f)
            this.transform.localScale = Vector3.one;
        else if (this.horizontal < -0.01f)
            this.transform.localScale = new Vector3(-1, 1, 1);


        // Set animator parameters
        this._animator.SetBool("walk", this.horizontal != 0);
        this._animator.SetBool("grounded", this.isGrounded());

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyUp(KeyCode.Space) && this._rigidbody.velocity.y > 0)
            this._rigidbody.velocity = new Vector2(this._rigidbody.velocity.x, this._rigidbody.velocity.y / 2);

        if (onWall())
        {
            this._rigidbody.gravityScale = 0f;
            this._rigidbody.velocity = Vector2.one;
        }
        else
        {
            this._rigidbody.gravityScale = 7f;
            this._rigidbody.velocity = new Vector2(this.horizontal * this.speed, this._rigidbody.velocity.y);

            if (isGrounded())
            {
                coyoteTimer = coyoteCooldown;
                this.jumpCounter = this.extraJumps;
            }
            else
                coyoteTimer -= Time.deltaTime;

        }

    }

    private void Jump()
    {
        if (coyoteTimer <= 0 && !onWall() && jumpCounter <= 0) return;

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                this._rigidbody.velocity = new Vector2(this._rigidbody.velocity.x, this.jumpPower);
            else
            {
                if (coyoteTimer > 0 )
                    this._rigidbody.velocity = new Vector2(this._rigidbody.velocity.x, this.jumpPower);
                else
                {
                    if (this.jumpCounter > 0)
                    {
                        this._rigidbody.velocity = new Vector2(this._rigidbody.velocity.x, this.jumpPower);
                        jumpCounter--;
                    }
                }
            }
            
            coyoteTimer = 0;
        }
    }

    private void WallJump()
    {
        this._rigidbody.AddForce(new Vector2 (-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;

    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(this._boxCollider.bounds.center, this._boxCollider.bounds.size, 0, Vector2.down, 0.1f, this._groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(this._boxCollider.bounds.center, this._boxCollider.bounds.size, 0, new Vector2(this.transform.localScale.x, 0), 0.1f, this._wallLayer);
        return raycastHit.collider != null;
    }

    public bool CanAttack()
    {
        return this.horizontal == 0 && this.isGrounded() && !this.onWall();
    }

}
