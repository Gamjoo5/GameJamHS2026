using UnityEngine;

public class FlintAndSteelTrigger : MonoBehaviour
{
    #region Unity Lifecycle

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController2D>(out var controller))
        {
            Debug.Log($"[FlintAndSteelTrigger] OnTriggerEnter2D with {other.gameObject.name}. Setting flint and steel flag to true.");
            controller.SetHasFlintAndSteel(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController2D>(out var controller))
        {
            Debug.Log($"[FlintAndSteelTrigger] OnCollisionEnter2D with {collision.gameObject.name}. Setting flint and steel flag to true.");
            controller.SetHasFlintAndSteel(true);
        }
    }

    #endregion
}