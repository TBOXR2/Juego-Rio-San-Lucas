using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla la barra de vida del personaje.
/// Pierde vida cuando un desecho escapa (lo llama el GameManager).
/// Si la vida llega a 0, el juego termina.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI (opcional, también se actualiza vía UIManager)")]
    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateBar();
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        UpdateBar();

        if (currentHealth <= 0f && GameManager.Instance != null)
            GameManager.Instance.GameOver("¡Te quedaste sin vida!");
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        UpdateBar();
    }

    void UpdateBar()
    {
        float fraction = currentHealth / maxHealth;

        if (healthSlider != null)
            healthSlider.value = fraction;

        if (GameManager.Instance != null && GameManager.Instance.ui != null)
            GameManager.Instance.ui.UpdateHealth(fraction);
    }
}
