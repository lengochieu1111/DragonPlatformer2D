using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifeTime;

    private BoxCollider2D boxCollider;
    private Animator animator;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        this.boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (this.hit) return;
        float movementSpeed = this.speed * Time.deltaTime * this.direction;
        this.transform.Translate(movementSpeed, 0, 0);

        this.lifeTime += Time.deltaTime;
        if (this.lifeTime > 5) 
            this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.hit = true;
        this.boxCollider.enabled = false;
        this.animator.SetTrigger("explode");

        if (collision.tag == "Enemy") 
        {
            collision.GetComponent<Health>().TakeDamage(1);
        }
    }

    public void SetDirection(float _direction)
    {
        this.lifeTime = 0;
        this.direction = _direction;
        this.gameObject.SetActive(true);
        this.hit = false;
        this.boxCollider.enabled = true;

        float localScale = this.transform.localScale.x;
        if (Mathf.Sign(localScale) != _direction)
            localScale = -localScale;

        this.transform.localScale = new Vector3(localScale, this.transform.localScale.y, this.transform.localScale.z);
        
    }

    private void Deactivate()
    {
        this.gameObject.SetActive(false);
    }


}
