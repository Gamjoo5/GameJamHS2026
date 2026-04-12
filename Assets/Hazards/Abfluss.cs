using UnityEngine;

public class Abfluss : MonoBehaviour
{
    [SerializeField] private Poison poison;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            poison.abflussVerstopft = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            poison.abflussVerstopft = false;
        }
    }
}
