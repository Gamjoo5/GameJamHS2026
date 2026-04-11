using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed;
    public Transform startingPoint;
    //public Transform[] points;
    public bool isStart = true;

    public Transform endpoint;

    private int i; //index of array
    private bool isMoving = false;

    void Start()
    {
        transform.position = startingPoint.position;
    }

    void Update()
    {
        if (!isMoving) return;


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

        transform.position = Vector2.MoveTowards(transform.position, !isStart ? endpoint.position : startingPoint.position, speed * Time.deltaTime);

    }

    public void startPlatform(){
        isMoving = true;
        isStart = !isStart;
    }

    public void stopPlatform()
    {
        isMoving = false;
    }
}
