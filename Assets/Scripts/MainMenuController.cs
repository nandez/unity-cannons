using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] protected Transform player;
    [SerializeField] private float playerBounceFrequency = 1f;
    [SerializeField] private float playerBounceAmplitude = 0.025f;

    [Header("Cannon Settings")]
    [SerializeField] protected ParticleSystem cannonSmokeEffect;
    [SerializeField] protected float cannonSmokeEffectInterval;
    [SerializeField] protected Transform cannonPivot;
    [SerializeField] protected float cannonRotationInterval;
    [SerializeField] protected float cannonRotationSpeed;

    [Header("Music Settings")]
    [SerializeField] protected AudioSource musicAudioSource;

    private Vector3 playerInitialPosition;
    private Vector3 cannonTarget;

    void Start()
    {
        // Preservamos la musica de fondo entre escenas..
        DontDestroyOnLoad(musicAudioSource.gameObject);

        // Guardamos la posición inicial del jugador para el efecto de bounce.
        playerInitialPosition = player.transform.position;

        // Seteamos los parámetros para los efectos del cañon..
        InvokeRepeating(nameof(PlaySmokeAnim), Random.Range(0, 2f), cannonSmokeEffectInterval);
        InvokeRepeating(nameof(SetCannonTarget), Random.Range(0, 2f), cannonRotationInterval);
    }

    void Update()
    {
        // Oscilación de la piedra
        var position = playerInitialPosition;
        position.y += Mathf.Sin(Time.fixedTime * Mathf.PI * playerBounceFrequency) * playerBounceAmplitude;
        player.transform.position = position;


        // Rotamos el pivote del cañon hacia el objetivo.
        var direction = cannonTarget - cannonPivot.position;
        direction.y = 0;
        var rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        cannonPivot.rotation = Quaternion.Lerp(cannonPivot.rotation, rotation, cannonRotationSpeed * Time.deltaTime);
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnExitGameButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void PlaySmokeAnim()
    {
        cannonSmokeEffect.Play();
    }

    private void SetCannonTarget()
    {
        cannonTarget = new Vector3(Random.Range(-20f, -0.05f), 0, Random.Range(-20, 20f)) * 1.5f;
    }
}
