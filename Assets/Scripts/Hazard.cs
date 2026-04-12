using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Sound Settings")]
    [Tooltip("The sound played when the player dies from this hazard.")]
    [SerializeField] private AudioClip deathSound;
    [Tooltip("The volume of the sound played.")]
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    [Header("References")]
    [Tooltip("The player's Rigidbody2D that triggers this hazard.")]
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
                PlayDeathSound();
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
                PlayDeathSound();
            }
        }
    }

    private void PlayDeathSound()
    {
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, volume);
        }
    }

    #endregion
}
