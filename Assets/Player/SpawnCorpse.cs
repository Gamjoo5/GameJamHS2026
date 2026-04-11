using System.Collections;
using UnityEngine;

public class SpawnCorpse : MonoBehaviour
{

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(Ragdoll());
    }

    private IEnumerator Ragdoll()
    {

        yield return new WaitForSeconds(1f);
        rb.bodyType = RigidbodyType2D.Static;
    }

}
