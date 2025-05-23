using UnityEngine;

public class WallManager : MonoBehaviour
{
    public GameObject topWall;
    public GameObject bottomWall;

    [Tooltip("Seconds for the walls to move towards the center.")]
    public float movementDuration = 5f; // e.g., 5 seconds
    [Tooltip("Distance from parent's Y-axis. Set 0 for to meet in the middle.")]
    public float finalDistanceFromCenter = 0.5f; // e.g., they stop 0.5 units from center

    private Vector3 topWallStartLocalPos;
    private Vector3 bottomWallStartLocalPos;
    private Vector3 topWallTargetLocalPos;
    private Vector3 bottomWallTargetLocalPos;

    private float _elapsedTime = 0f;
    private bool _isMoving = false;

    void Start()
    {
        topWallStartLocalPos = topWall.transform.localPosition;
        bottomWallStartLocalPos = bottomWall.transform.localPosition;

        topWallTargetLocalPos = new Vector3(topWallStartLocalPos.x, finalDistanceFromCenter, topWallStartLocalPos.z);
        bottomWallTargetLocalPos = new Vector3(bottomWallStartLocalPos.x, -finalDistanceFromCenter, bottomWallStartLocalPos.z);

        StartWallMovement();
    }

    public void StartWallMovement()
    {
        _elapsedTime = 0f;
        _isMoving = true;
        Debug.Log("Wall movement initiated.");
    }

    public void ResetWallsToStart()
    {
        _isMoving = false;
        _elapsedTime = 0f;
        if (topWall != null) topWall.transform.localPosition = topWallStartLocalPos;
        if (bottomWall != null) bottomWall.transform.localPosition = bottomWallStartLocalPos;
        Debug.Log("Walls reset to their initial positions.");
    }

    void Update()
    {
        if (!_isMoving)
        {
            return;
        }

        _elapsedTime += Time.deltaTime;

        float t = _elapsedTime / movementDuration;

        t = Mathf.Clamp(t, 0, 1);

        topWall.transform.localPosition = Vector3.Lerp(topWallStartLocalPos, topWallTargetLocalPos, t);
        bottomWall.transform.localPosition = Vector3.Lerp(bottomWallStartLocalPos, bottomWallTargetLocalPos, t);

        // --- Check for Completion ---
        if (t >= 1f)
        {
            _isMoving = false; // Stop the movement once it's complete
            Debug.Log("Wall movement completed.");

            // Optional: Snap to final position to ensure perfect alignment
            topWall.transform.localPosition = topWallTargetLocalPos;
            bottomWall.transform.localPosition = bottomWallTargetLocalPos;
        }
    }
}