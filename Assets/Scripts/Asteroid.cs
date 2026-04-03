using System;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float maxSpeed = 5f;

    [SerializeField] private float maxX = 9.2f;
    [SerializeField] private float maxY = 5.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.position = new Vector3(UnityEngine.Random.Range(-maxX, maxX), UnityEngine.Random.Range(-maxY, maxY), 0);

        rb.linearVelocity = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)) * new Vector3(UnityEngine.Random.Range(0.5f, maxSpeed), 0.0f, 0.0f);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
