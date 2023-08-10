using System.Collections;
using UnityEngine;

/**
 * A class used to manager spawn for every game object
 */
public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerups;

    private bool _stopSpawning;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    /**
     * Every five seconds spawn an enemy with a random x position
     */
    private IEnumerator SpawnEnemyRoutine()
    {
        while (!_stopSpawning)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector3 posToSpawn = new Vector3(randomX, 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }

    }

    private IEnumerator SpawnPowerupRoutine()
    {
        while (!_stopSpawning)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector3 posToSpawn = new Vector3(randomX, 7, 0);
            GameObject randomPowerup = _powerups[Random.Range(0, 2)];
            Instantiate(randomPowerup, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5, 8));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
