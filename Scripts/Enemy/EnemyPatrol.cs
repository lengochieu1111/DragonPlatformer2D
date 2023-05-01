using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    [SerializeField] private Vector3 initScale;
    [SerializeField] private bool movingLeft = false;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration = 1f;
    private float idleTimer = 0;

    [Header("Enemy Animators")]
    [SerializeField] private Animator _animator;


    private void OnDisable()
    {
        _animator.SetBool("moving", false);
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        initScale = enemy.localScale;
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MovementInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MovementInDirection(1);
            else
                DirectionChange();
        }

    }

    private void DirectionChange()
    {
        _animator.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MovementInDirection(float _directiion)
    {
        idleTimer = 0;
        _animator.SetBool("moving", true);

        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _directiion, initScale.y, initScale.z);

        enemy.position = new Vector3(enemy.position.x + _directiion * speed * Time.deltaTime, enemy.position.y, enemy.position.z);
    }

}
