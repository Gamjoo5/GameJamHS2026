using System.Collections;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public bool abflussVerstopft;
    public Transform targetpoint;
    public int speed;
    [SerializeField] private GameObject zufuhr;


    private bool isRising = false;

    private void Start()
    {
        abflussVerstopft = false;
    }

    private void Update()
    {
        if (isRising)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, targetpoint.position, speed * Time.deltaTime);

            if ((Vector2)transform.position == (Vector2)targetpoint.position)
                isRising = false;
        }
    }

    public void Rise()
    {
        Debug.Log(abflussVerstopft);
        StartCoroutine(SpawnZufuhr());
        if (abflussVerstopft)
            isRising = true;
    }

    private IEnumerator SpawnZufuhr()
    {
        zufuhr.SetActive(true);
        yield return new WaitForSeconds(2);
        zufuhr.SetActive(false);
    }
}
