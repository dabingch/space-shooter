using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player is null)
        {
            Debug.LogError("Player is null");
        }

        _anim = GetComponent<Animator>();
        if (_anim is null)
        {
            Debug.LogError("Anim is null");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // If enemy is out of bound, then reposition
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string collidorTag = other.tag;

        if (collidorTag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player)
            {
                player.Damage();
            }
        }
        else if (collidorTag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player is not null)
            {
                _player.AddScore(10);
            }
        }

        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        Destroy(this.gameObject, 2.8f);
    }
}
