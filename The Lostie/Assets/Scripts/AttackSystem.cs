using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject[] shellPrefabs; // Array of projectile prefabs
    public GameObject[] shellPrefabs_2; // Array of projectile prefabs
    public float shellSpeed = 10f;
    public string attackButton = "Fire1";
    public float detectionRadius = 10f;

    [Header("Enemy Settings")]
    public LayerMask enemyLayer;
    public Material enemyHighlightMaterial;

    private GameObject currentTargetEnemy;
    private Material originalEnemyMaterial;
    private int currentProjectileIndex = 0;
    private int currentProjectileIndex_2 = 0;

    ThirdPersonCameraController cameraController;
    private void Start()
    {
        cameraController = FindObjectOfType<ThirdPersonCameraController>();
    }
    private void Update()
    {
        UpdateNearestEnemy();

        if (Input.GetButtonDown(attackButton) && currentTargetEnemy != null && cameraController.aimingMode)
        {
            SpawnShellAndShoot_2(currentTargetEnemy);
        }

        CheckProjectileSelectionInput();
        CheckProjectileSelectionInput_2();
    }

    private void CheckProjectileSelectionInput()
    {
        if (cameraController != null && !cameraController.aimingMode)
        {
            for (int i = 1; i <= shellPrefabs.Length; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    currentProjectileIndex = i - 1;

                    if (currentTargetEnemy != null)
                    {
                        SpawnShellAndShoot(currentTargetEnemy);
                    }
                }
            }
        }
    }
    private void CheckProjectileSelectionInput_2()
    {
        if (cameraController != null && cameraController.aimingMode)
        {
            for (int i = 1; i <= shellPrefabs_2.Length; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    currentProjectileIndex_2 = i - 1;
                }
            }
        }
    }

    private void UpdateNearestEnemy()
    {
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != currentTargetEnemy)
        {
            if (currentTargetEnemy != null)
            {
                RemoveHighlight(currentTargetEnemy);
            }

            currentTargetEnemy = nearestEnemy;

            if (currentTargetEnemy != null)
            {
                HighlightEnemy(currentTargetEnemy);
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        if (enemiesInRange.Length == 0)
        {
            return null;
        }

        Collider nearestEnemy = enemiesInRange[0];
        float minDistance = Vector3.Distance(transform.position, nearestEnemy.transform.position);

        foreach (Collider enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy.gameObject;
    }

    private void SpawnShellAndShoot(GameObject targetEnemy)
    {
        GameObject shell = Instantiate(shellPrefabs[currentProjectileIndex], transform.position, Quaternion.identity);
        shell.GetComponent<Rigidbody>().velocity = (targetEnemy.transform.position - transform.position).normalized * shellSpeed;
        shell.AddComponent<ProjectileBehavior>();
    }
    private void SpawnShellAndShoot_2(GameObject targetEnemy)
    {
        GameObject shell = Instantiate(shellPrefabs_2[currentProjectileIndex_2], transform.position, Quaternion.identity);
        shell.GetComponent<Rigidbody>().velocity = (targetEnemy.transform.position - transform.position).normalized * shellSpeed;
        shell.AddComponent<ProjectileBehavior>();
    }

    private void HighlightEnemy(GameObject enemy)
    {
        Renderer enemyRenderer = enemy.GetComponent<Renderer>();

        if (enemyRenderer != null)
        {
            originalEnemyMaterial = enemyRenderer.material;
            enemyRenderer.material = enemyHighlightMaterial;
        }
    }

    private void RemoveHighlight(GameObject enemy)
    {
        Renderer enemyRenderer = enemy.GetComponent<Renderer>();

        if (enemyRenderer != null)
        {
            enemyRenderer.material = originalEnemyMaterial;
        }
    }
}
