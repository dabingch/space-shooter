using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _horizontalBounds = 13.0f;
    private float _verticalBounds = 3.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_laserPrefab, transform.position, Quaternion.identity); // Quaternion means default value
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float vertialInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, vertialInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        var verticalBounds = Mathf.Clamp(transform.position.y, _verticalBounds * -1, _verticalBounds);
        transform.position = new Vector3(transform.position.x, verticalBounds, 0);

        if (transform.position.x > _horizontalBounds)
        {
            transform.position = new Vector3(_horizontalBounds * -1, transform.position.y, 0);
        }
        else if (transform.position.x < _horizontalBounds * -1)
        {
            transform.position = new Vector3(_horizontalBounds, transform.position.y, 0);
        }
    }
}
