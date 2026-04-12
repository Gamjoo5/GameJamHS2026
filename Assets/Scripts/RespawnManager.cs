using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("Sound Settings")]
    [Tooltip("The sound played when all enemies are respawned.")]
    [SerializeField] private AudioClip respawnSound;
    [Tooltip("The volume of the sound played.")]
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

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
        PlayRespawnSound();
    }

    private void PlayRespawnSound()
    {
        if (respawnSound != null)
        {
            AudioSource.PlayClipAtPoint(respawnSound, transform.position, volume);
        }
    }
}
