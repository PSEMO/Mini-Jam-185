using UnityEngine;
using System.Collections;

public class MoveHorizontalSingleDirection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private float horizontalMoveSpeed = 5f;

    [SerializeField]
    private float horizontalRange = 5f;

    private float startPosX;
    [SerializeField] private int horizontalDirection = 1;

    [SerializeField]
    private float verticalNudgeAmount = 0.75f;

    [SerializeField]
    private float verticalNudgeSpeed = 3f;

    private Coroutine currentNudgeCoroutine;

    void Start()
    {
        startPosX = transform.position.x;
    }

    void Update()
    {
        float targetX = transform.position.x + (horizontalDirection * horizontalMoveSpeed * Time.deltaTime);

        targetX = Mathf.Clamp(targetX, startPosX - horizontalRange, startPosX + horizontalRange);

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        
    }

    private IEnumerator NudgeVertical(float targetDisplacementY)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(startPos.x, startPos.y + targetDisplacementY, startPos.z);

        // Calculate duration based on nudge amount and speed
        float duration = Mathf.Abs(targetDisplacementY) / verticalNudgeSpeed;
        if (duration == 0) yield break; // Avoid division by zero if targetDisplacementY is 0

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // 't' goes from 0 to 1
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos;
    }
}
