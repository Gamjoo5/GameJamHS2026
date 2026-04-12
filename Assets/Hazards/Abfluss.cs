using System.Collections;
using UnityEngine;

public class Abfluss : MonoBehaviour
{
    [SerializeField] private Poison poison;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathObject"))
        {
            poison.abflussVerstopft = true;
            Debug.Log("Abfluss verstopft");
        }
    }
}
