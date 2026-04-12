using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private VictoryUI victoryUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && victoryUI != null)
        {
            victoryUI.Zeigen();
        }
    }
}
