using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;

    private float _lowerBoundOfEnemy = -5f;
    private float _initalYPosOfEnemy = 7f;
    private float _fireRate = 3f;
    private float _canFire = -1;

    [SerializeField]
    private GameObject _laserPrefab;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>(); // player instance
        if (_player is null)
        {
            Debug.LogError("Player is null");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource is null)
        {
            Debug.LogError("Audio Source is null");
        }

        _anim = GetComponent<Animator>();
        if (_anim is null)
        {
            Debug.LogError("Anim is null");
        }
    }

    void Update()
    {
        CalculateMovement();
        EnemeyFireLaser();
    }

    private void EnemeyFireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            foreach (Laser laser in lasers)
            {
                laser.AssignEnemyLaser();
            }
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime); // Move up per second

        // If enemy is out of bound, then reposition
        if (transform.position.y < _lowerBoundOfEnemy)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, _initalYPosOfEnemy, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string collidorTag = other.tag;

        if (collidorTag == "Player")
        {
            _player.Damage();
            DestoryEnemy();
        }
        else if (collidorTag == "Laser")
        {
            Destroy(other.gameObject);
            _player.AddScore(10);
            DestoryEnemy();
        }
    }

    private void DestoryEnemy()
    {
        _anim.SetTrigger("OnEnemyDeath"); // Trigger explosion anim
        _speed = 0; // Freeze enemy
        _audioSource.Play(); // Play explosion sound
        Destroy(GetComponent<Collider2D>()); // Prevent damaging player
        Destroy(this.gameObject, 2.8f); // Wait for the explosion anim and sound
    }
}
