using UnityEngine;

public class FlintAndSteelTrigger : MonoBehaviour
{
    [Header("Sound Settings")]
    [Tooltip("The sound played when the player enters this trigger.")]
    [SerializeField] private AudioClip enterSound;
    [Tooltip("The volume of the sound played.")]
    [SerializeField, Range(0f, 10f)] private float volume = 1f;

    #region Unity Lifecycle

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController2D>(out var controller))
        {
            Debug.Log($"[FlintAndSteelTrigger] OnTriggerEnter2D with {other.gameObject.name}. Setting flint and steel flag to true.");
            controller.SetHasFlintAndSteel(true);
            PlayEnterSound();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController2D>(out var controller))
        {
            Debug.Log($"[FlintAndSteelTrigger] OnCollisionEnter2D with {collision.gameObject.name}. Setting flint and steel flag to true.");
            controller.SetHasFlintAndSteel(true);
            PlayEnterSound();
        }
    }

    private void PlayEnterSound()
    {
        if (enterSound != null)
        {
            AudioSource.PlayClipAtPoint(enterSound, transform.position, volume);
        }
    }

    #endregion
}