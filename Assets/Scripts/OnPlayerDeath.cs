using UnityEngine;

public class OnPlayerDeath : MonoBehaviour, IOnDeath
{

    [Header("References")]
    [Tooltip("Manager responsible for respawning enemies when the player dies.")]
    [SerializeField] private RespawnManager respawnManager;

    private PlayerController2D playerController;
    

    public void OnDeath()
    {
        playerController.Die();
        respawnManager.RespawnAllEnemies();

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
