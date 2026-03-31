using UnityEngine;

public class SpaceShip : MonoBehaviour
{

    private float turnSpeed = 45;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float turnAngle;
        if(Input.GetKey("a")){
            turnAngle = turnSpeed * Time.deltaTime;
            transform.Rotate(0, 0, turnAngle);
        }

        if (Input.GetKey("d")) {
            turnAngle = -1 * turnSpeed * Time.deltaTime;
            transform.Rotate(0, 0, turnAngle);
        }
    }
}
