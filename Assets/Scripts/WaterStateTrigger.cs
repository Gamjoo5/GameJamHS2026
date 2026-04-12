using UnityEngine;

public class WaterStateTrigger : MonoBehaviour
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
            Debug.Log($"[WaterStateTrigger] OnTriggerEnter2D with {other.gameObject.name}. Setting player water state to Falling.");
            controller.SetWaterState(PlayerController2D.WaterState.Falling);
            PlayEnterSound();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController2D>(out var controller))
        {
            Debug.Log($"[WaterStateTrigger] OnCollisionEnter2D with {collision.gameObject.name}. Setting player water state to Falling.");
            controller.SetWaterState(PlayerController2D.WaterState.Falling);
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
