using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla el menú principal: botón de Inicio y botón de Salida.
/// Coloca este script en un objeto vacío de la escena "Menu"
/// y conecta los botones a estos métodos (OnClick).
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Tooltip("Nombre EXACTO de la escena del juego")]
    public string gameSceneName = "Juego";

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); // en el Editor no cierra, solo lo verás en el build final
    }
}
