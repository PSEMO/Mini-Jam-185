using UnityEngine;
using System.Collections;
public class MoveLeftRight : MonoBehaviour
{
    [SerializeField]
    private float horizontalMoveSpeed = 5f;

    [SerializeField]
    private float horizontalRange = 5f;

    private float startPosX;
    private int horizontalDirection = 1;

    [SerializeField]
    private float verticalNudgeAmount = 0.75f;

    [SerializeField]
    private float verticalNudgeSpeed = 3f;

    private Coroutine currentNudgeCoroutine;

    void Start()
    {
        startPosX = transform.position.x;

        if (Random.value < 0.5f)
        {
            horizontalDirection = -1;
        }
        else
        {
            horizontalDirection = 1;
        }
    }

    void Update()
    {
        float targetX = transform.position.x + (horizontalDirection * horizontalMoveSpeed * Time.deltaTime);

        targetX = Mathf.Clamp(targetX, startPosX - horizontalRange, startPosX + horizontalRange);

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        if (horizontalDirection == 1 && transform.position.x >= startPosX + horizontalRange)
        {
            horizontalDirection = -1;
        }
        else if (horizontalDirection == -1 && transform.position.x <= startPosX - horizontalRange)
        {
            horizontalDirection = 1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (currentNudgeCoroutine != null)
            {
                StopCoroutine(currentNudgeCoroutine);
            }

            float nudgeDirection = 0;

            if (transform.position.y < other.bounds.center.y)
            {
                nudgeDirection = -1f;
            }
            else
            {
                nudgeDirection = 1f;
            }

            currentNudgeCoroutine = StartCoroutine(NudgeVertical(nudgeDirection * verticalNudgeAmount));
        }
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