using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player Attack")]
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject[] fireballs;
    [SerializeField] protected float cooldownTimer = Mathf.Infinity;

    [Header("Player Animators")]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected PlayerMovement playerMovement;

    [Header("Audio Attack")]
    [SerializeField] protected AudioClip fireballSound;

    private void Awake()
    {
        this._animator = GetComponent<Animator>();
        this.playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && this.cooldownTimer > this.attackCooldown && this.playerMovement.CanAttack())
            this.Attack();

        this.cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        this.cooldownTimer = 0;
        SoundManager.instance.PlaySound(fireballSound);
        this._animator.SetTrigger("attack");

        this.fireballs[FindFireball()].transform.position = this.firePoint.position;
        this.fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(this.transform.localScale.x));

    }

    private int FindFireball()
    {
        for(int i = 0; i < this.fireballs.Length; i++)
        {
            if (!this.fireballs[i].activeInHierarchy)
                return i;
        }

        return 0;
    }


}
