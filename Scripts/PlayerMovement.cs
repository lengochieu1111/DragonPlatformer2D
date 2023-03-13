using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : DragonMonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rigidbody;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected BoxCollider2D _boxCollider;
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected LayerMask _wallLayer;
    [SerializeField] protected bool grounded;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float jumpPower = 10f;
    [SerializeField] protected float horizontal;
    [SerializeField] protected float wallJumpCoolDown;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigidbody();
        this.LoadAnimator();
        this.LoadBoxCollider();
    }

    protected virtual void LoadRigidbody()
    {
        if (this._rigidbody != null) return;
        this._rigidbody = GetComponent<Rigidbody2D>();

        Debug.LogWarning(transform.name + " : LoadRigidbody ", gameObject);
    }
    
    protected virtual void LoadAnimator()
    {
        if (this._animator != null) return;
        this._animator = GetComponent<Animator>();

        Debug.LogWarning(transform.name + " : LoadAnimator ", gameObject);
    }
    
    protected virtual void LoadBoxCollider()
    {
        if (this._boxCollider != null) return;
        this._boxCollider = GetComponent<BoxCollider2D>();

        Debug.LogWarning(transform.name + " : LoadBoxCollider ", gameObject);
    }

    protected virtual void Update()
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

        if (this.wallJumpCoolDown < 0.2f)
        {
            this._rigidbody.velocity = new Vector2(this.horizontal * this.speed, this._rigidbody.velocity.y);

            if(this.onWall() && !this.isGrounded())
            {
                this._rigidbody.gravityScale = 0;
                this._rigidbody.velocity = Vector2.zero;
            }
            else
                this._rigidbody.gravityScale = 7;

            if (Input.GetKey(KeyCode.Space))
                this.Jump();

        }
        else
            this.wallJumpCoolDown += Time.deltaTime;

    }

    protected virtual void Jump()
    {
        if (this.isGrounded())
        {
            this._rigidbody.velocity = new Vector2(this._rigidbody.velocity.x, this.jumpPower);
            this._animator.SetTrigger("jump");
        }
        else if (this.onWall() && !this.isGrounded())
        {
            if (this.horizontal == 0)
            {
                this._rigidbody.velocity = new Vector2(-Mathf.Sign(this.transform.localScale.x) * 10, 0);
                this.transform.localScale = new Vector3(-Mathf.Sign(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
            }
            else
                this._rigidbody.velocity = new Vector2(-Mathf.Sign(this.transform.localScale.x) * 3, 6);

            this.wallJumpCoolDown = 0;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    protected virtual bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(this._boxCollider.bounds.center, this._boxCollider.bounds.size, 0, Vector2.down, 0.1f, this._groundLayer);
        return raycastHit.collider != null;
    }
    
    protected virtual bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(this._boxCollider.bounds.center, this._boxCollider.bounds.size, 0, new Vector2(this.transform.localScale.x, 0), 0.1f, this._wallLayer);
        return raycastHit.collider != null;
    }

}
