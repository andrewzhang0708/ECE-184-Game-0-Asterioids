using UnityEngine;

public class Spaceship : MonoBehaviour
{
    private float turnSpeed = 180f;

    [SerializeField] private float thrust = 5f;

    private Vector3 shipDirection = new Vector3(0, 1, 0);

    private Rigidbody2D rb;

    [SerializeField] private float screenLeft = -10f;
    [SerializeField] private float screenRight = 10f;
    [SerializeField] private float screenTop = 5f;
    [SerializeField] private float screenBottom = -5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void WrapAround()
    {
        Vector3 pos = transform.position;

        if (pos.x > screenRight)
        {
            pos.x = screenLeft;
        }
        else if (pos.x < screenLeft)
        {
            pos.x = screenRight;
        }

        if (pos.y > screenTop)
        {
            pos.y = screenBottom;
        }
        else if (pos.y < screenBottom)
        {
            pos.y = screenTop;
        }

        transform.position = pos;
    }

    void Update()
    {
        float turnAngle = 0f;

        // Rotation (A / D)
        if (Input.GetKey(KeyCode.A))
        {
            turnAngle = turnSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            turnAngle = -turnSpeed * Time.deltaTime;
        }

        // Apply rotation
        transform.Rotate(0, 0, turnAngle);

        // Update direction vector (THIS is what your tutorial is teaching)
        shipDirection = Quaternion.Euler(0, 0, turnAngle) * shipDirection;

        // Thrust (W)
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(shipDirection * thrust);
        }

        WrapAround();
    }
}