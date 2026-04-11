using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Rigidbody2D player;

    #region Unity Lifecycle

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody == player && player != null)
        {
            if (player.TryGetComponent<PlayerController2D>(out var controller))
            {
                Debug.Log($"[Hazard] OnCollisionEnter2D with player: {collision.gameObject.name}. Killing player.");
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
                Debug.Log($"[Hazard] OnTriggerEnter2D with player: {other.gameObject.name}. Killing player.");
                controller.Die();
            }
        }
    }

    #endregion
}
