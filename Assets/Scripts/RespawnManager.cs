using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private RespawnableEnemy[] enemies;

    void Start()
    {
        enemies = FindObjectsOfType<RespawnableEnemy>();
    }

    public void RespawnAllEnemies()
    {
        foreach (RespawnableEnemy enemy in enemies)
        {
            enemy.Respawn();
        }
    }
}
