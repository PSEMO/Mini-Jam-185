using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameplaySong : MonoBehaviour
{
    public int nextSceneIndex = 2;

    // --- Singleton Pattern --- (Just in case)
    public static StartGameplaySong Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"[{gameObject.name}] will persist across scenes.");
        }
        else
        {
            // If an instance already exists, destroy this duplicate.
            Debug.LogWarning($"[{gameObject.name}] - Duplicate instance detected. Destroying this one.");
            Destroy(gameObject);
            return;
        }

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        Debug.Log($"Loading scene by index: {nextSceneIndex}");
        SceneManager.LoadScene(nextSceneIndex);
    }
}
