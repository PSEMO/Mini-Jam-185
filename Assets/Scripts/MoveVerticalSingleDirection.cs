using UnityEngine;
using UnityEngine.Rendering;

public class MoveVerticalSingleDirection : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float currentDirection = 1; // 1 up, -1 down
    private Renderer objectRenderer;
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();    
    }


    void Update()
    {
        if (objectRenderer.isVisible)
        {
            transform.Translate(Vector3.up * currentDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void ReverseDirection()
    {
        currentDirection *= -1f;
    }
}
