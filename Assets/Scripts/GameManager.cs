using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] protected List<Transform> spawnPoints;
    [SerializeField] protected GameObject player;
    [SerializeField] protected Camera mainCamera;
    [SerializeField] protected Vector3 mainCameraInitialOffset;

    [Header("Quest Settings")]
    [SerializeField] protected TextMeshProUGUI questText;
    [SerializeField] protected AudioSource questUpdateSound;

    // Events
    public UnityEvent OnGameOver;
    public UnityEvent OnGamePaused;
    public UnityEvent OnGameResumed;
    public UnityEvent OnLevelCompleted;

    // Properties
    public GameState State { get; protected set; }

    // Fields
    public int totalTowers;
    public int towersTakenDown;

    void Start()
    {
        // Buscamos todos los objetos destruibles en la escena...
        var towers = GameObject.FindObjectsOfType<TowerController>();
        totalTowers = towers?.Length ?? 0;
        if (towers?.Length > 0)
            foreach (TowerController obj in towers)
                obj.OnTowerDestroyed += OnTowerDestroyed;

        // Actualizamos el texto de la quest.
        UpdateQuestText();

        // Asignamos el handler para el evento de muerte del jugador.
        player.GetComponent<HealthController>().OnDeath += OnPlayerDeath;

        // Posicionamos al jugador en un punto de spawn aleatorio.
        var spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        player.transform.position = spawnPoint.position;

        // Actualizamos la posición de la cámara para que inicialmente apunte al jugador.
        var camPosition = mainCamera.transform.position;
        camPosition.x = spawnPoint.position.x + mainCameraInitialOffset.x;
        camPosition.y = camPosition.y + mainCameraInitialOffset.y;
        camPosition.z = spawnPoint.position.z + mainCameraInitialOffset.z;
        mainCamera.transform.position = camPosition;

        // Seteamos el estado del juego
        State = GameState.Running;
        Time.timeScale = 1;
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
        Application.Quit();
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
        Time.timeScale = 0;
        OnGameOver?.Invoke();
    }

    private void OnQuestCompleted()
    {
        State = GameState.GameOver;
        Time.timeScale = 0;
        OnLevelCompleted?.Invoke();
    }

    private void OnTowerDestroyed()
    {
        // Cuando se destruye una torre, actualizamos el contador, ejecutamos el sonido
        // y actualizamos y el texto de la quest. Si ademmás no quedan torres por destruir
        // finalizamos el juego.
        towersTakenDown++;
        questUpdateSound.Play();
        UpdateQuestText();
        if (towersTakenDown >= totalTowers)
            OnQuestCompleted();
    }

    private void UpdateQuestText()
    {
        questText.SetText($"Destroy all cannon towers. {Environment.NewLine}{Environment.NewLine}{towersTakenDown}/{totalTowers}");
    }

    public enum GameState
    {
        Running,
        Paused,
        GameOver
    }
}
