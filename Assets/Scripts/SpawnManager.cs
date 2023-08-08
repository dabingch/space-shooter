using System.Collections;
using UnityEngine;

/**
 * A class used to communicate with player and enemy
 */
public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {

    }

    /**
     * Every five seconds spawn an enemy with a random x position
     */
    private IEnumerator SpawnRoutine()
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

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
