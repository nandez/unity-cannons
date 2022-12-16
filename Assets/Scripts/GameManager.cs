using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] protected HealthController playerHealthCtrl;

    public UnityEvent OnGameOver;
    public UnityEvent OnGamePaused;
    public UnityEvent OnGameResumed;
    public UnityEvent OnLevelCompleted;

    public GameState State { get; protected set; }

    void Start()
    {
        State = GameState.Running;

        playerHealthCtrl.OnDeath += OnPlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (State == GameState.Running)
            {
                PauseGame();
            }
            else if (State == GameState.Paused)
            {
                ResumeGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnPlayerDeath();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            OnQuestCompleted();
        }
    }


    public void PauseGame()
    {
        if (State == GameState.Running)
        {
            Time.timeScale = 0;
            State = GameState.Paused;
            OnGamePaused?.Invoke();
        }
    }

    public void ResumeGame()
    {
        if (State == GameState.Paused)
        {
            Time.timeScale = 1;
            State = GameState.Running;
            OnGameResumed?.Invoke();
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit()
#endif
    }

    public void RestartGame()
    {
        // Cargamos la escena actual.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnPlayerDeath()
    {
        State = GameState.GameOver;
        OnGameOver?.Invoke();
    }

    private void OnQuestCompleted()
    {
        State = GameState.GameOver;
        OnLevelCompleted?.Invoke();
    }


    public enum GameState
    {
        Running,
        Paused,
        GameOver
    }
}
