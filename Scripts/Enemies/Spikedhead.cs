using UnityEngine;

public class Spikedhead : EnemyDamage
{
    [Header("Spikedhead")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float range = 16f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float attackTimer = Mathf.Infinity;
    private bool attacking = false;

    private Vector3[] directions = new Vector3[4];
    private Vector3 destination;

    
    private void OnEnable()
    {
        Stop();
    }

    private void Update()
    {
        if (attacking)
            transform.Translate(destination * speed * Time.deltaTime);
        else
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackDelay)
                CheckForPlayer();
        }
    }

    private void CheckForPlayer()
    {
        CalculateDirections();

        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                attackTimer = 0;
            }
        }
    }

    private void CalculateDirections()
    {
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range;
    }

    private void Stop()
    {
        destination = transform.position;
        attacking = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Stop();
    }

    /*
    private void Start()
    {
        Stop();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackDelay) CheckForPlayer();
        transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            destination = collision.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Stop();

        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < directions.Length; i++)
            Gizmos.DrawLine(transform.position, directions[i]);
    }
    private void CheckForPlayer()
    {
        CalculateDirections();

        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], Mathf.Infinity, playerLayer);

            if (hit.collider != null)
            {
                destination = directions[i];
                attackTimer = 0;
            }
        }
    }

    private void CalculateDirections()
    {
        directions[0] = transform.position - transform.right * range; // left
        directions[1] = transform.position + transform.right * range; // right
        directions[2] = transform.position - transform.up * range;    // down
        directions[3] = transform.position + transform.up * range;    // up
    }

    private void Stop()
    {
        destination = transform.position;
        attackTimer = 0;
    }
    */


}
