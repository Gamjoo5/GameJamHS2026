using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [Header("Settings")]
    public UnityEvent onButtonPressed;
    public UnityEvent onButtonReleased;

    [Header("Sprites & Visuals")]
    [Tooltip("Sprite shown when the button is pressed.")]
    public Sprite buttonpressed;
    [Tooltip("Sprite shown when the button is released.")]
    public Sprite buttonreleased;

    private SpriteRenderer sr;

    private int objectsOnButton = 0;
    public bool isPressed => objectsOnButton > 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("DeathObject"))
        {
            objectsOnButton++;
            if (objectsOnButton == 1)
            {
                PressButton();
            }
        }
    }

    void PressButton()
    {
        if (buttonpressed) sr.sprite = buttonpressed;
        onButtonPressed?.Invoke();
        Debug.Log("Button gedrückt!");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("DeathObject"))
        {
            objectsOnButton--;
            if (objectsOnButton <= 0)
            {
                objectsOnButton = 0;
                ReleaseButton();
            }
        }
    }

    void ReleaseButton()
    {
        if (buttonreleased) sr.sprite = buttonreleased;
        onButtonReleased?.Invoke();
        Debug.Log("Button losgelassen!");
    }
}
