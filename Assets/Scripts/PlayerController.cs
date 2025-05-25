using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    public GameObject LevelWon;
    public GameObject GameEnded;
    public Image HPImg;

    public float MaxHP = 100;
    public float CurrentHP = 100;
    public float WallDamage = 100;
    public float BulletDamage = 5;
    public float OtherDamage = 10;

    public float speed = 10f;
    public float verticalSpeed = 5f;

    public float upperLimit = 10f;
    public float lowerLimit = -10f;

    public float tiltAngle = 15f;
    public float tiltSpeed = 90f;

    public float invulnerabilityDuration = 2.0f;
    public float shakeDuration = 2.0f;
    public float blinkInterval = 0.1f;
    public float shakeMagnitude = 0.1f;

    bool isInvulnerable = false;
    float invulnerabilityTimer = 0f;

    Renderer playerRenderer;

    Coroutine blinkCoroutine;
    Coroutine shakeCoroutine;

    float dt = 0;

    bool isAlive = true;

    public AudioResource[] audioResources = new AudioResource[4];
    AudioSource audioSource;

    void Awake()
    {
        playerRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Time.timeScale = 1.0f;
    }

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

        float targetZAngleDegrees = verticalInput * tiltAngle;
        Quaternion targetWorldRoll = Quaternion.Euler(0, 0, targetZAngleDegrees);
        Vector3 targetUpForRoll = targetWorldRoll * Vector3.up;

        float stepRadians = tiltSpeed * Mathf.Deg2Rad * Time.deltaTime;
        Vector3 newObjectUp = Vector3.RotateTowards(transform.up, targetUpForRoll, stepRadians, 0.0f);

        transform.rotation = Quaternion.LookRotation(Vector3.forward, newObjectUp);

        if (isInvulnerable)
        {
            invulnerabilityTimer -= dt;
            if (invulnerabilityTimer <= 0)
            {
                EndInvulnerability();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            AddHP(-WallDamage);
            if (isAlive)
            {
                EndInvulnerability();
                StartInvulnerability();
            }
            else
            {
                StopAllCoroutines();
            }
        }

        if (!isInvulnerable)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                AddHP(-BulletDamage);
                if (isAlive)
                {
                    StartInvulnerability();
                }
                Destroy(other.gameObject);
            }
            if (other.gameObject.CompareTag("Enemy"))
            {
                AddHP(-OtherDamage);
                if (isAlive)
                {
                    StartInvulnerability();
                }
            }
        }
        if(other.gameObject.CompareTag("LevelWon"))
        {
            LevelWon.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    void AddHP(float ammount)
    {
        CurrentHP += ammount;
        HPImg.fillAmount = CurrentHP / MaxHP;

        if(ammount < 0)
        {
            audioSource.resource = audioResources[Random.Range(0, 4)];
            audioSource.Stop();
            audioSource.Play();
        }

        if (CurrentHP <= 0)
        {
            GameEnded.SetActive(true);
            Time.timeScale = 0.0f;
            isAlive = false;
        }
    }

    void StartInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;

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

        while (Time.time < shakeEndTime && isInvulnerable)
        {
            float xOffset = Random.Range(-1f, 1f) * shakeMagnitude;
            float yOffset = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position += new Vector3(xOffset, yOffset, 0);

            yield return null;
        }

        Debug.Log("Shake ended.");
    }
}