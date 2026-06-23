using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Cerebro del juego. Controla:
/// - El sistema de niveles (máximo 3) y el temporizador de cada nivel.
/// - El aumento progresivo de dificultad cada 2 minutos.
/// - La contaminación del río y las condiciones de victoria/derrota.
/// - El puntaje.
/// Es un Singleton: cualquier script puede llamarlo con GameManager.Instance.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Sistema de Niveles")]
    public int maxLevels = 3;
    public float levelDuration = 120f;   // 2 minutos por nivel (cámbialo a 15 para probar rápido)
    private int currentLevel = 1;
    private float levelTimer;

    [Header("Dificultad (se escala por nivel)")]
    public float baseSpawnInterval = 2f;     // segundos entre desechos en el nivel 1
    public float baseWasteSpeed = 2f;        // velocidad de caída en el nivel 1
    public float spawnIntervalStep = 0.5f;   // cuánto se ACORTA el intervalo por nivel
    public float wasteSpeedStep = 1f;        // cuánto SUBE la velocidad por nivel

    [Header("Contaminación del Río (0 - 100)")]
    public float contamination = 0f;
    public float maxContamination = 100f;
    public float contaminationPerMiss = 10f; // sube cuando un desecho escapa
    public float contaminationPerCatch = 4f; // baja cuando capturas un desecho

    [Header("Vida del Jugador")]
    public PlayerHealth playerHealth;
    public float damagePerMiss = 8f;         // daño al jugador cuando un desecho escapa

    [Header("Puntaje")]
    public int score = 0;
    public int pointsPerCatch = 10;

    [Header("Referencias de Escena")]
    public UIManager ui;
    public RiverContamination river;

    [HideInInspector] public bool isGameOver = false;
    [HideInInspector] public bool isPaused = false;

    void Awake()
    {
        // Garantiza que solo exista un GameManager
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        currentLevel = 1;
        levelTimer = levelDuration;
        contamination = 0f;
        score = 0;
        isGameOver = false;
        isPaused = false;
        Time.timeScale = 1f;
        UpdateUI();
        if (river != null) river.UpdateColor(0f);
    }

    void Update()
    {
        if (isGameOver || isPaused) return;

        // Temporizador del nivel: cuenta regresiva
        levelTimer -= Time.deltaTime;
        if (levelTimer <= 0f)
            AdvanceLevel();

        if (ui != null) ui.UpdateTimer(levelTimer);
    }

    void AdvanceLevel()
    {
        if (currentLevel < maxLevels)
        {
            currentLevel++;            // sube de nivel -> más velocidad y frecuencia
            levelTimer = levelDuration; // reinicia el temporizador
        }
        else
        {
            // Si sobrevive todo el nivel 3: ¡gana!
            WinGame();
        }
        UpdateUI();
    }

    // ---------- Dificultad calculada según el nivel actual ----------
    public float CurrentSpawnInterval()
    {
        float interval = baseSpawnInterval - (currentLevel - 1) * spawnIntervalStep;
        return Mathf.Max(0.4f, interval); // nunca menos de 0.4 s
    }

    public float CurrentWasteSpeed()
    {
        return baseWasteSpeed + (currentLevel - 1) * wasteSpeedStep;
    }

    public int CurrentLevel() => currentLevel;

    // ---------- Contaminación ----------
    public void AddContamination(float amount)
    {
        contamination = Mathf.Clamp(contamination + amount, 0f, maxContamination);
        if (river != null) river.UpdateColor(contamination / maxContamination);
        UpdateUI();

        if (contamination >= maxContamination)
            GameOver("¡El Río San Lucas quedó totalmente contaminado!");
    }

    public void ReduceContamination(float amount)
    {
        contamination = Mathf.Clamp(contamination - amount, 0f, maxContamination);
        if (river != null) river.UpdateColor(contamination / maxContamination);
        UpdateUI();
    }

    // ---------- Puntaje ----------
    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    // ---------- Eventos de desechos ----------
    public void OnWasteCaught()
    {
        AddScore(pointsPerCatch);
        ReduceContamination(contaminationPerCatch);
    }

    public void OnWasteMissed()
    {
        AddContamination(contaminationPerMiss);
        if (playerHealth != null) playerHealth.TakeDamage(damagePerMiss);
    }

    // ---------- Estados del juego ----------
    public void GameOver(string reason)
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;
        if (ui != null) ui.ShowGameOver(reason);
    }

    public void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;
        if (ui != null) ui.ShowWin(score);
    }

    public void TogglePause()
    {
        if (isGameOver) return;
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        if (ui != null) ui.ShowPause(isPaused);
    }

    void UpdateUI()
    {
        if (ui == null) return;
        ui.UpdateLevel(currentLevel);
        ui.UpdateScore(score);
        ui.UpdateContamination(contamination / maxContamination);
    }
}
