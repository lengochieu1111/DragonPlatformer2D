using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private float range = 1f;
    [SerializeField] private float boxColliderDistance = 1f;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;


    [Header("Colliders Parameters")]
    [SerializeField] private Animator _animator;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private LayerMask playerLayer;

    [Header("Player Layer")]
    [SerializeField] private EnemyPatrol enemyPatrol;

    [Header("Audio Attack")]
    [SerializeField] protected AudioClip fireballSound;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                _animator.SetTrigger("rangedAttack");
            }

        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    private void RangedAttack()
    {
        cooldownTimer = 0;
        SoundManager.instance.PlaySound(fireballSound);
        fireballs[FindFire()].transform.position = firepoint.position;
        fireballs[FindFire()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFire()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_boxCollider.bounds.center + transform.right * boxColliderDistance * transform.localScale.x,
            new Vector3(_boxCollider.bounds.size.x * range, _boxCollider.bounds.size.y, _boxCollider.bounds.size.z), 0, Vector2.right, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCollider.bounds.center + transform.right * boxColliderDistance * transform.localScale.x,
            new Vector3(_boxCollider.bounds.size.x * range, _boxCollider.bounds.size.y, _boxCollider.bounds.size.z));
    }

}
