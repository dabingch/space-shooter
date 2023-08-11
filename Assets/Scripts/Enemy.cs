using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
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

        Destroy(this.gameObject);
    }
}
