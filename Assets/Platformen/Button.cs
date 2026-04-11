using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent onButtonPressed;
    public UnityEvent onButtonReleased;

    [Header("Texturen")]
    public Sprite buttonpressed;
    public Sprite buttonreleased;

    private SpriteRenderer sr;

    public bool isPressed = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !isPressed)
        {
            isPressed = true;
            if (buttonpressed) sr.sprite = buttonpressed;
            onButtonPressed?.Invoke();
            Debug.Log("Button von Player gedrückt!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isPressed)
        {
            isPressed = false;
            if (buttonreleased) sr.sprite = buttonreleased;
            onButtonReleased?.Invoke();
            Debug.Log("Button losgelassen!");
        }
    }    
}
