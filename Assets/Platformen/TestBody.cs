using System.Collections;
using UnityEngine;

public class TestBody : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform startPosition;

    public float resetDelay = 3f;

    void Start()
    {
        transform.position = startPosition.position;
        StarteReset();
    }

    // Update is called once per frame
    public void StarteReset()
    {
        StartCoroutine(ResetNachXSekunden());
    }

    private IEnumerator ResetNachXSekunden()
    {
        yield return new WaitForSeconds(resetDelay);
        transform.position = startPosition.position; 
    }
}
