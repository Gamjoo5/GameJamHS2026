using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Settings")]
    public float speed;
    public Transform startingPoint;
    public Transform endpoint;

    [Header("References")]
    public Button relButton;

    private int i;
    //private bool isMoving = false;

    void Start()
    {
        transform.position = startingPoint.position;
    }

    void Update()
    {
        
        /*
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        */
        //Vector2.Distance(transform.position, startingPoint.position) < 0.02f

        transform.position = Vector2.MoveTowards(transform.position, relButton.isPressed ? endpoint.position : startingPoint.position, speed * Time.deltaTime);
    }

    public void startPlatform(){
        //isMoving = true;
    }
}
