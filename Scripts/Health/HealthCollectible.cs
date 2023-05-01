using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float valueHealth;

    [Header("Audio Pickup")]
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(pickupSound);
            collision.GetComponent<Health>().AddHealth(valueHealth);
            gameObject.SetActive(false);
        }
    }
}
