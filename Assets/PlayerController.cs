using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float verticalSpeed = 5f;
    public float upperLimit = 10f;
    public float lowerLimit = -10f;

    float dt = 0;

    void Update()
    {
        dt = Time.deltaTime;

        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            verticalInput = -1f;
        }

        float newY = Mathf.Clamp(transform.position.y + (verticalInput * verticalSpeed * dt), lowerLimit, upperLimit);

        transform.position = new Vector3(transform.position.x + (speed * dt), newY, 0);
    }
}