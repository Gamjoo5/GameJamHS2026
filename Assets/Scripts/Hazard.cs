using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] Rigidbody2D player;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody == player && player != null)
        {
            if (player.TryGetComponent<PlayerController2D>(out var controller))
            {
                controller.Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == player && player != null)
        {
            if (player.TryGetComponent<PlayerController2D>(out var controller))
            {
                controller.Die();
            }
        }
    }
}
