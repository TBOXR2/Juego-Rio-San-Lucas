using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Maneja la pausa dentro del juego (tecla ESC) y los botones del canvas:
/// Reanudar, Reiniciar, Salir al menú y Salir del juego.
/// Coloca este script en un objeto vacío de la escena "Juego".
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Tooltip("Nombre EXACTO de la escena del menú")]
    public string menuSceneName = "Menu";

    void Update()
    {
        // ESC para pausar / reanudar
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance != null)
            GameManager.Instance.TogglePause();
    }

    // --- Botones del canvas ---

    public void Resume()
    {
        if (GameManager.Instance != null) GameManager.Instance.TogglePause();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
