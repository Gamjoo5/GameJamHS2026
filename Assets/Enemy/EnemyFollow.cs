using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed;
    private Rigidbody2D rb;

    public enum State { Idle, Follow };

    public State currentState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = State.Idle;
    }

   

    private void FixedUpdate()
    {
        if (currentState == State.Follow)
        {
            MoveToPlayer();
        }
    }

    private void MoveToPlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocityY);
    }
}
