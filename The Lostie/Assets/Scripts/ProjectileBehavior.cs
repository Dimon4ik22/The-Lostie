using System.Collections;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float lifeTime = 3f;
    public float damage = 1f;
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            float damageToDeal = damage;

            switch (enemyHealth.uniqueIdentifier)
            {
                case "TypeA":
                    damageToDeal *= 2;
                    break;
                case "TypeB":
                    damageToDeal *= 3;
                    break;
                default:
                    break;
            }

            enemyHealth.TakeDamage(damageToDeal);
            Destroy(gameObject);
            enemyHealth.isUsed = false;
        }
    }
}