using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private EnemyFollow follow;


    private void Start()
    {
        follow = GetComponentInParent < EnemyFollow >();
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            follow.currentState = EnemyFollow.State.Follow;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            follow.currentState = EnemyFollow.State.Idle;
        }
    }
}
