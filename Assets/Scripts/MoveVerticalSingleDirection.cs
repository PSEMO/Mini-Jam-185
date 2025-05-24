using UnityEngine;

public class MoveVerticalSingleDirection : MonoBehaviour
{
    private float moveSpeed = 5f;

    [SerializeField] private float currentDirection = 1; // 1 up, -1 down

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.up * currentDirection * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);                     
    }

    private void ReverseDirection()
    {
        currentDirection *= -1f;
    }
}
