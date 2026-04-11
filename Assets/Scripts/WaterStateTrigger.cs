using UnityEngine;

public class WaterStateTrigger : MonoBehaviour
{
    #region Unity Lifecycle

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController2D>(out var controller))
        {
            Debug.Log($"[WaterStateTrigger] OnTriggerEnter2D with {other.gameObject.name}. Setting player water state to Falling.");
            controller.SetWaterState(PlayerController2D.WaterState.Falling);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController2D>(out var controller))
        {
            Debug.Log($"[WaterStateTrigger] OnCollisionEnter2D with {collision.gameObject.name}. Setting player water state to Falling.");
            controller.SetWaterState(PlayerController2D.WaterState.Falling);
        }
    }

    #endregion
}
