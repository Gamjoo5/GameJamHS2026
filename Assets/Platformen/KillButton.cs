using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class KillButton : MonoBehaviour
{
    public UnityEvent onButtonPressed;
    public UnityEvent onButtonReleased;

    [Header("Texturen")]
    public Sprite buttonpressed;
    public Sprite buttonreleased;

    [Header("Timer Settings")]
    public float resetDelay = 5f;

    private SpriteRenderer sr;

    public bool isPressed = false;
    private bool isKillPending = false;
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
            
            if (!isKillPending)
            {
                killPlayer();
            }
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
        isKillPending = true;
        StartCoroutine(KillNachXSekunden());
    }

    private IEnumerator KillNachXSekunden()
    {
        float remainingTime = resetDelay;
        
        while (remainingTime > 0)
        {
            if (player != null && player.IsOnWater)
            {
                Debug.Log("[KillButton] Player is on water, stopping kill timer.");
                isKillPending = false;
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
            remainingTime -= 0.1f;
        }

        player.Die();
        
        // Wait a small bit before resetting pending flag to avoid immediate re-triggering if respawned on button
        yield return new WaitForSeconds(0.5f);
        isKillPending = false;
    }   
}
