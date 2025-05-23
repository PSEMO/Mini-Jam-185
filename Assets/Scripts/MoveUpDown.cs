using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    private float moveSpeed = 5f;

    private float currentDirection = 0; // 1 up, -1 down

    void Start()
    {
        currentDirection = 1f;
        if (Random.Range(0,1) > 0.5f) currentDirection = -1f;
    }

    void Update()
    {
        transform.Translate(Vector3.up * currentDirection * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ReverseDirection();
        }
    }

    private void ReverseDirection()
    {
        currentDirection *= -1f;
    }
}