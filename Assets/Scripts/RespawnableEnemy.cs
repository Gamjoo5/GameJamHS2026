using UnityEngine;

public class RespawnableEnemy : MonoBehaviour, IOnDeath
{
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    public void Respawn()
    {
        transform.position = startPosition;
        gameObject.SetActive(true);
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void OnDeath()
    {
        gameObject.SetActive(false);
    }
}
