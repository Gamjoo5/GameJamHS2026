using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damageAmount = 1;

    /*

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }

    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }
}
