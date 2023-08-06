using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private float _limitToDestoryPrefab = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y + _speed * Time.deltaTime, 0);
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > _limitToDestoryPrefab)
        {
            Destroy(this.gameObject);
        }
    }
} 
