using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GenericButton : MonoBehaviour
{
    [Header("Buttons")]
    public UnityEvent onButtonPressed;
    public UnityEvent onButtonReleased;

    public UnityEvent onDelayedAction;
    public float resetDelay = 1f;


    [Header("Texturen")]
    public Sprite buttonpressed;
    public Sprite buttonreleased;

    public float resetDelay = 1f;

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
            StartMethod();
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

    private void StartMethod()
    {
        StartCoroutine(StartNachXSekunden());
    }

    private IEnumerator StartNachXSekunden()
    {
        yield return new WaitForSeconds(resetDelay);
        onDelayedAction?.Invoke();
    }   
}
