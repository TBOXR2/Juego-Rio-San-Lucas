using UnityEngine;

/// <summary>
/// (VERSIÓN 3D) Cambia gradualmente el color del Material del río de azul
/// (limpio) a verde (contaminado), a partir de un umbral. Compatible con
/// el render Built-in (_Color) y con URP/HDRP (_BaseColor).
/// El GameManager llama a UpdateColor cuando cambia la contaminación.
/// </summary>
public class RiverContamination : MonoBehaviour
{
    [Header("Renderer del río (el plano del agua)")]
    public Renderer riverRenderer;

    [Header("Colores")]
    public Color cleanColor    = new Color(0.30f, 0.60f, 1.00f); // azul limpio
    public Color pollutedColor = new Color(0.20f, 0.80f, 0.20f); // verde contaminado

    [Header("Umbral crítico (0-1)")]
    [Range(0f, 1f)]
    public float threshold = 0.4f; // recién a partir de aquí empieza a ponerse verde

    private Material mat;

    void Start()
    {
        if (riverRenderer == null)
            riverRenderer = GetComponent<Renderer>();
        if (riverRenderer != null)
            mat = riverRenderer.material; // crea una instancia editable del material
        UpdateColor(0f);
    }

    /// <param name="t">0 = río limpio, 1 = contaminación máxima</param>
    public void UpdateColor(float t)
    {
        if (mat == null) return;

        Color c;
        if (t < threshold)
            c = cleanColor;
        else
        {
            float k = (t - threshold) / (1f - threshold);
            c = Color.Lerp(cleanColor, pollutedColor, k);
        }

        SetColor(c);
    }

    void SetColor(Color c)
    {
        // Asigna el color sin importar el render pipeline en uso.
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", c); // URP / HDRP
        if (mat.HasProperty("_Color"))     mat.SetColor("_Color", c);     // Built-in
    }
}
