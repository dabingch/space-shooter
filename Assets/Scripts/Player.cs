using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _horizontalBounds = 13.0f;
    private float _verticalBounds = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = .15f; // Time for fire cool down
    private float _canFire = -1f; // Time used to compared with the current time 
    private float _laserPosOffset = 1.05f;
    [SerializeField]
    private bool _isTripleShootActive = false;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    void Start()
    {   
        // Initial player position
        transform.position = new Vector3(0, 0, 0);
        // Find and access the SpawnManager script
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_spawnManager is null)
        {
            Debug.LogError("The spawn manager is null");
        }
    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShootActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserPosOffset, 0), Quaternion.identity); // Quaternion means default value
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float vertialInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, vertialInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        // Set up vertical movement bounds
        var verticalBounds = Mathf.Clamp(transform.position.y, _verticalBounds * -1, _verticalBounds);
        transform.position = new Vector3(transform.position.x, verticalBounds, 0);

        // Set up horizontal movement bounds
        if (transform.position.x > _horizontalBounds)
        {
            transform.position = new Vector3(_horizontalBounds * -1, transform.position.y, 0);
        }
        else if (transform.position.x < _horizontalBounds * -1)
        {
            transform.position = new Vector3(_horizontalBounds, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        _lives--;

        if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShootActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShootActive = false;
    }
}
