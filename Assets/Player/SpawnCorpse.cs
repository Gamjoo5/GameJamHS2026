using System.Collections;
using UnityEngine;

public class SpawnCorpse : MonoBehaviour
{

    [Header("Sound Settings")]
    [Tooltip("The sound looped while the corpse is burning.")]
    [SerializeField] private AudioClip burningSound;
    [Tooltip("The volume of the sound played.")]
    [SerializeField, Range(0f, 10f)] private float volume = 1f;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer burnSpriteRenderer;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (burnSpriteRenderer == null)
        {
            Transform burningTransform = transform.Find("Burning");
            if (burningTransform != null)
            {
                burnSpriteRenderer = burningTransform.GetComponent<SpriteRenderer>();
            }
        }
        
        if (burnSpriteRenderer != null)
        {
            burnSpriteRenderer.enabled = false;
        }
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(Ragdoll());
    }

    public void ShowBurnSprite()
    {
        if (burnSpriteRenderer != null)
        {
            burnSpriteRenderer.enabled = true;
        }

        if (burningSound != null)
        {
            AudioSource.PlayClipAtPoint(burningSound, transform.position, volume);
        }
    }

    private IEnumerator Ragdoll()
    {

        yield return new WaitForSeconds(1f);
        rb.bodyType = RigidbodyType2D.Static;
    }

}
