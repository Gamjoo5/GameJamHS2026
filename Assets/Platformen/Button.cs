using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent onButtonPressed;
    public UnityEvent onButtonReleased;
    private bool isPressed = false;

    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Button pressed");
        if(other.CompareTag("Player") && !isPressed)
        {
            isPressed = true;
            onButtonPressed?.Invoke();
            Debug.Log("Button von Player gedrückt!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isPressed)
        {
            isPressed = false;
            onButtonReleased?.Invoke();
            Debug.Log("Button losgelassen!");
        }
    }



    
}
