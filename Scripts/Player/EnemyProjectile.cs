using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float resetTime = 3f;
    private float lifetime = 5f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        gameObject.SetActive(false);
    }

    public void ActivateProjectile()
    {
        lifetime = 0;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime) 
            gameObject.SetActive(false);
    }

}
