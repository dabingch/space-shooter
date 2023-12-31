using System.Collections;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2f;
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

    private bool _isTripleShootActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    [SerializeField]
    private bool _isPlayerOne;
    [SerializeField]
    private bool _isPlayerTwo;

    private Animator _anim;

    void Start()
    {
        // Find and access the SpawnManager script
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager is null)
        {
            Debug.LogError("The Spawn Manager is null");
        }

        // Find canvas and access the UIManager script
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager is null)
        {
            Debug.LogError("The UI Manager is null");
        }

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gameManager is null)
        {
            Debug.LogError("Game Manager is null");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource is null)
        {
            Debug.LogError("Audio Source on the player is null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if (!_gameManager.IsCoOpMode)
        {
            // Initial player position
            transform.position = new Vector3(0, 0, 0);
        }

        _anim = GetComponent<Animator>();
        if (_anim is null)
        {
            Debug.LogError("Animator is null");
        }
    }

    void Update()
    {
        CalculateMovement();
        PlayerTurnAnim();
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

        _audioSource.Play();
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

    private void PlayerTurnAnim()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _anim.SetBool("isTurningLeft", true);
            _anim.SetBool("isTurningRight", false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _anim.SetBool("isTurningLeft", false);
            _anim.SetBool("isTurningRight", true);
        }
        else
        {
            _anim.SetBool("isTurningLeft", false);
            _anim.SetBool("isTurningRight", false);
        }
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
        
        _uiManager.UpdateLives(_lives);

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

    public void SpeedBoostActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerdownRoutine());
    }

    private IEnumerator SpeedPowerdownRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
