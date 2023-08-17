using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private float _boundsToDestroyPrefab = 8.0f;
    private bool _isEnemyLaser = false;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource is null)
        {
            Debug.LogError("Audio Source is null");
        }
    }

    void Update()
    {
        if (!_isEnemyLaser)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime); // Move up

        if (transform.position.y > _boundsToDestroyPrefab)
        {
            // Check if this object has parent, destroy the parent too
            if (transform.parent is not null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime); // Move up

        if (transform.position.y < _boundsToDestroyPrefab * -1)
        {
            if (transform.parent is not null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser)
        {
            Player player = other.GetComponent<Player>();
            if (player is not null)
            {
                player.Damage();
                _audioSource.Play();
            }
        }
    }
}
