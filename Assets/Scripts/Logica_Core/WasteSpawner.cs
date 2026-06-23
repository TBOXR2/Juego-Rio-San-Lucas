using UnityEngine;

/// <summary>
/// (VERSIÓN 3D) Genera desechos al inicio del río (Z lejano) en posiciones X
/// aleatorias. El intervalo se acorta conforme sube el nivel (mayor frecuencia)
/// y además tiene variación aleatoria.
/// </summary>
public class WasteSpawner : MonoBehaviour
{
    [Header("Prefabs de desechos (botella, bolsa, residuo...)")]
    public GameObject[] wastePrefabs;

    [Header("Zona de aparición (inicio del río, lejos del jugador)")]
    public float spawnZ = 12f;   // qué tan lejos aparecen (eje Z)
    public float spawnY = 0.5f;  // altura sobre la superficie del río
    public float minX = -6f;     // límite X izquierdo
    public float maxX = 6f;      // límite X derecho

    private float timer;

    void Update()
    {
        if (GameManager.Instance == null ||
            GameManager.Instance.isGameOver ||
            GameManager.Instance.isPaused)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnWaste();
            // Intervalo según el nivel + variación aleatoria.
            float interval = GameManager.Instance.CurrentSpawnInterval();
            timer = interval * Random.Range(0.7f, 1.3f);
        }
    }

    void SpawnWaste()
    {
        if (wastePrefabs == null || wastePrefabs.Length == 0) return;

        GameObject prefab = wastePrefabs[Random.Range(0, wastePrefabs.Length)];
        float x = Random.Range(minX, maxX);
        Instantiate(prefab, new Vector3(x, spawnY, spawnZ), Quaternion.identity);
    }
}
