using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public string uniqueIdentifier;
    public bool isUsed = true;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isUsed)
        {
            currentHealth -= damage;
            Debug.Log(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
        StartCoroutine(HitTimer());
        
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(0.01f);
        isUsed = true;
    }
}
