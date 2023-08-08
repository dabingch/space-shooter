using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private float _boundsToDestroyPrefab = 8.0f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > _boundsToDestroyPrefab)
        {
            Destroy(this.gameObject);
        }
    }
} 
