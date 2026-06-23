using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro (cuando agregues un texto, usa "Text - TextMeshPro")

/// <summary>
/// Centraliza todos los elementos visuales del HUD (barras, textos, paneles).
/// Los demás scripts llaman a estos métodos para actualizar la pantalla.
/// Arrastra cada referencia en el Inspector.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Barras (Slider)")]
    public Slider healthBar;
    public Slider contaminationBar;

    [Header("Textos (TextMeshPro)")]
    public TMP_Text timerText;
    public TMP_Text levelText;
    public TMP_Text scoreText;

    [Header("Paneles")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public TMP_Text gameOverReasonText;
    public TMP_Text winScoreText;

    public void UpdateHealth(float fraction)
    {
        if (healthBar != null) healthBar.value = fraction;
    }

    public void UpdateContamination(float fraction)
    {
        if (contaminationBar != null) contaminationBar.value = fraction;
    }

    public void UpdateTimer(float seconds)
    {
        if (timerText == null) return;
        if (seconds < 0) seconds = 0;
        int m = Mathf.FloorToInt(seconds / 60f);
        int s = Mathf.FloorToInt(seconds % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", m, s);
    }

    public void UpdateLevel(int level)
    {
        if (levelText != null) levelText.text = "Nivel " + level;
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null) scoreText.text = "Puntos: " + score;
    }

    public void ShowPause(bool show)
    {
        if (pausePanel != null) pausePanel.SetActive(show);
    }

    public void ShowGameOver(string reason)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameOverReasonText != null) gameOverReasonText.text = reason;
    }

    public void ShowWin(int score)
    {
        if (winPanel != null) winPanel.SetActive(true);
        if (winScoreText != null) winScoreText.text = "Puntaje final: " + score;
    }
}
