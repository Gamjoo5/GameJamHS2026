using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;
    private IOnDeath death;

    void Start()
    {
        currentHealth = maxHealth;
        death = GetComponent<IOnDeath>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        Debug.Log(gameObject.name + " took " + amount + " damage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died");
        death.OnDeath();
    }

}
