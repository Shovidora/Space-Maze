using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    public int levelsCompleted = 0;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
