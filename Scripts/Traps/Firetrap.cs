using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

    [Header("Firetrap Timers")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField] private float activationDelay = 2f;
    [SerializeField] private float activeTime = 2f;

    [SerializeField] private bool triggered = false;
    [SerializeField] private bool active = false;
    [SerializeField] private Health playerHealth;

    [Header("Audio Firetrap")]
    [SerializeField] private AudioClip soundFiretrap;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerHealth != null && active)
            playerHealth.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();

            if (!triggered) 
                StartCoroutine(ActivateFiretrap());

            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerHealth = null;

    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        spriteRend.color = Color.red;

        yield return new WaitForSeconds(activationDelay);
        active = true;
        SoundManager.instance.PlaySound(soundFiretrap);
        spriteRend.color = Color.white;
        animator.SetBool("activated", true);

        yield return new WaitForSeconds(activeTime); 
        triggered = false;
        active = false;
        animator.SetBool("activated", false);
    }

}
