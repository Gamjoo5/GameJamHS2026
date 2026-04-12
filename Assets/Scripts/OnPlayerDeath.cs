using UnityEngine;

public class OnPlayerDeath : MonoBehaviour, IOnDeath
{

    [Header("Sound Settings")]
    [Tooltip("The sound played when the player dies.")]
    [SerializeField] private AudioClip deathSound;
    [Tooltip("The volume of the sound played.")]
    [SerializeField, Range(0f, 10f)] private float volume = 1f;

    [Header("References")]
    [Tooltip("Manager responsible for respawning enemies when the player dies.")]
    [SerializeField] private RespawnManager respawnManager;

    private PlayerController2D playerController;
    

    public void OnDeath()
    {
        playerController.Die();
        respawnManager.RespawnAllEnemies();
        PlayDeathSound();
    }

    private void PlayDeathSound()
    {
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, volume);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
