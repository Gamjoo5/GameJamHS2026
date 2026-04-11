using System.Net;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public bool abflussVerstopft = true;
    public Transform targetpoint;
    public int speed;

    private void Start()
    {
        abflussVerstopft = true;
    }

    public void Rise()
    {
        Debug.Log(abflussVerstopft);
        if (abflussVerstopft) 
        {
            transform.position = Vector2.MoveTowards(transform.position, targetpoint.position, speed * Time.deltaTime);
        }
    }
}
