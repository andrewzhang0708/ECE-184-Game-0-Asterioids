//using System;
//using System.Diagnostics;
using UnityEngine;
// using static System.Net.Mime.MediaTypeNames;

public class GameController : MonoBehaviour
{
    private enum GameState { Intro, Playing, GameOver }
    private GameState currentState = GameState.Intro;

    [Header("Asteroid Settings")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private int baseAsteroidCount = 8;
    [SerializeField] private float spawnDistance = 12f;

    [Header("Player / Lives")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int startingLives = 3;

    [Header("UI")]
    [SerializeField] private GameObject[] lifeIcons;
    [SerializeField] private GameObject introPanel;     // Assign in Inspector
    [SerializeField] private GameObject gameOverPanel;  // Assign in Inspector

    private int currentLevel = 1;
    private int asteroidsRemaining;

    private int currentLives;
    private GameObject currentPlayer;

    void Start()
    {
        ShowIntro();
    }

    void Update()
    {
        if (currentState == GameState.Intro)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                StartGame();
            }
        }
        else if (currentState == GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                StartGame(); // Restart game
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Quitting Game...");
                Application.Quit(); // Note: This only works in a built game, not in the editor
            }
        }
    }

    // ---------------- STATE MANAGEMENT ----------------

    void ShowIntro()
    {
        currentState = GameState.Intro;
        introPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f; // Pause the game
    }

    void StartGame()
    {
        currentState = GameState.Playing;
        introPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        CleanUpScene();

        currentLevel = 1;
        currentLives = startingLives;
        Time.timeScale = 1f; // Unpause

        UpdateLivesUI();
        SpawnPlayer();
        StartLevel();
    }

    void CleanUpScene()
    {
        // Cancel any pending invokes (like respawning or next level delays)
        CancelInvoke();

        // Destroy any leftover asteroids
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject asteroid in asteroids)
        {
            Destroy(asteroid);
        }

        // Destroy player if one already exists
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }
    }

    // ---------------- LEVEL ----------------

    void StartLevel()
    {
        int numToSpawn = baseAsteroidCount + 1 << (currentLevel - 1); // 1,2,4,8,...
        asteroidsRemaining = numToSpawn;

        for (int i = 0; i < numToSpawn; i++)
        {
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = new Vector3(dir.x, dir.y, 0) * spawnDistance;

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        Vector2 moveDir = (-spawnPos).normalized;

        rb.linearVelocity = moveDir * Random.Range(1.5f, 3f);
        rb.angularVelocity = Random.Range(-50f, 50f);
    }

    public void AsteroidDestroyed()
    {
        asteroidsRemaining--;

        if (asteroidsRemaining <= 0 && currentState == GameState.Playing)
        {
            currentLevel++;
            Invoke(nameof(StartLevel), 1.5f);
        }
    }

    // ---------------- PLAYER / LIVES ----------------

    void SpawnPlayer()
    {
        currentPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }

    public void PlayerDied()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives > 0)
        {
            Invoke(nameof(SpawnPlayer), 1.5f); // respawn delay
        }
        else
        {
            GameOver();
        }
    }

    void UpdateLivesUI()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (i < currentLives)
            {
                lifeIcons[i].SetActive(true);
            }
            else
            {
                lifeIcons[i].SetActive(false);
            }
        }
    }

    void GameOver()
    {
        currentState = GameState.GameOver;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
}