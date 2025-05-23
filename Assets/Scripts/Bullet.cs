using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 10f;

    public float lifeTime = 3f;
    private float currentLifeTimer;

    public Vector3 movementDirection;

    void Start()
    {
        currentLifeTimer = lifeTime;

        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        }
        else
        {
            Debug.LogWarning("Bullet movementDirection not set! Defaulting to Vector3.right.", this);
            movementDirection = Vector3.right;
            transform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        }
    }

    void Update()
    {
        transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);

        currentLifeTimer -= Time.deltaTime;

        if (currentLifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}