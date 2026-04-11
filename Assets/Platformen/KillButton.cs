using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class KillButton : MonoBehaviour
{
    public UnityEvent onButtonPressed;
    public UnityEvent onButtonReleased;

    [Header("Texturen")]
    public Sprite buttonpressed;
    public Sprite buttonreleased;

    public float resetDelay = 5f;

    private SpriteRenderer sr;

    public bool isPressed = false;
    public PlayerController2D player;

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
            killPlayer();
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

    private void killPlayer()
    {
        StartCoroutine(KillNachXSekunden());
    }

    private IEnumerator KillNachXSekunden()
    {
        yield return new WaitForSeconds(resetDelay);
        player.Die();
    }   
}
