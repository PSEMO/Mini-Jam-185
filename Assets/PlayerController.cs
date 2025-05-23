using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Image HPImg;

    public float MaxHP = 100;
    public float CurrentHP = 100;
    public float Damage = 10;

    public float speed = 10f;
    public float verticalSpeed = 5f;
    public float upperLimit = 10f;
    public float lowerLimit = -10f;

    public float invulnerabilityDuration = 2.0f;
    public float shakeDuration = 2.0f;
    public float blinkInterval = 0.1f;
    public float shakeMagnitude = 0.05f;

    bool isInvulnerable = false;
    float invulnerabilityTimer = 0f;

    Renderer playerRenderer;

    Vector3 positionAtStartOfInvulnerability;
    Coroutine blinkCoroutine;
    Coroutine shakeCoroutine;

    float dt = 0;

    void Awake()
    {
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer == null)
        {
            playerRenderer = GetComponent<SpriteRenderer>();
        }
        if (playerRenderer == null)
        {
            Debug.LogError("PlayerController: No Renderer or SpriteRenderer found on this object for blinking!");
        }
    }

    void Update()
    {
        Debug.Log(transform.position);

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

        if (isInvulnerable)
        {
            invulnerabilityTimer -= dt;
            if (invulnerabilityTimer <= 0)
            {
                EndInvulnerability();
            }
        }
    }

    // Using OnTriggerEnter, ensure colliders are set up as triggers
    private void OnTriggerEnter(Collider other)
    {
        if (!isInvulnerable && (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Enemy")))
        {
            Debug.Log("Hit: " + other.gameObject.name + " - Becoming invulnerable!");
            StartInvulnerability();
            AddHP(-Damage);
        }
    }

    void AddHP(float ammount)
    {
        CurrentHP += ammount;
        HPImg.fillAmount = CurrentHP / MaxHP;
    }    

    void StartInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
        positionAtStartOfInvulnerability = transform.position; // Store where invulnerability began

        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);

        blinkCoroutine = StartCoroutine(BlinkEffect());
        shakeCoroutine = StartCoroutine(ShakeEffect());
    }

    void EndInvulnerability()
    {
        isInvulnerable = false;
        if (playerRenderer != null)
        {
            playerRenderer.enabled = true;
        }

        Debug.Log("Invulnerability ended.");
    }

    IEnumerator BlinkEffect()
    {
        if (playerRenderer == null) yield break;

        float blinkEndTime = Time.time + invulnerabilityDuration;

        while (Time.time < blinkEndTime && isInvulnerable)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
        playerRenderer.enabled = true;
    }

    IEnumerator ShakeEffect()
    {
        float shakeEndTime = Time.time + shakeDuration;

        // Loop as long as the shake duration hasn't passed AND the player is still invulnerable
        while (Time.time < shakeEndTime && isInvulnerable)
        {
            float xOffset = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
            float yOffset = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position += new Vector3(xOffset, yOffset, 0);

            yield return null;
        }

        Debug.Log("Shake ended.");
    }
}