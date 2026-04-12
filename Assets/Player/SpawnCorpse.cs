using System.Collections;
using UnityEngine;

public class SpawnCorpse : MonoBehaviour
{

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
    }

    private IEnumerator Ragdoll()
    {

        yield return new WaitForSeconds(1f);
        rb.bodyType = RigidbodyType2D.Static;
    }

}
