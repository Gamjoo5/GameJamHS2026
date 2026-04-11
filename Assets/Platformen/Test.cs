using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Test : MonoBehaviour
{
    public Transform[] points;
    public bool buttonPressed;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        InputAsk();

        if (buttonPressed)
        {
            MovePlatform(points[0]);
        }
        else
        {
            MovePlatform(points[1]);
        }
    }

    private void InputAsk()
    {
        if (Input.GetKey(KeyCode.Space)) {
            buttonPressed = true;
        } else 
            buttonPressed = false;
    }

    private void MovePlatform(Transform targetPosition)
    {
        Debug.Log("Move to " + targetPosition.position);
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPosition.position, 20 * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }
}
