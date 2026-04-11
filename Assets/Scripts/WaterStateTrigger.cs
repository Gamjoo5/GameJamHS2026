using UnityEngine;

public class WaterStateTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController2D>(out var controller))
        {
            controller.SetWaterState(PlayerController2D.WATER_STATE.FALLING);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController2D>(out var controller))
        {
            controller.SetWaterState(PlayerController2D.WATER_STATE.FALLING);
        }
    }
}
