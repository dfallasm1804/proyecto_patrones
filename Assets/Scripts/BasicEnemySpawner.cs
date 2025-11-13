using System;
using System.Collections;
using Interfaces;
using UnityEngine;


public class BasicEnemySpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private GameObject enemyPrefab;
    private float spawnInterval = 3.5f;
    private void Start()
    {
        StartCoroutine(Spawn(spawnInterval, enemyPrefab));
    }

    private IEnumerator Spawn(float spawnInterval, GameObject enemy)
    {
        yield return new WaitForSeconds(spawnInterval);
        GameObject newEnemy = Instantiate(enemy, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        StartCoroutine(Spawn(spawnInterval, enemy));
    }
}
