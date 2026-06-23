using UnityEngine;

/// <summary>
/// (VERSIÓN 3D) Comportamiento de un desecho. "Fluye" por el río hacia el
/// jugador (por defecto en -Z, hacia la cámara). Si toca al Player -> capturado.
/// Si toca la DeadZone (detrás del jugador) -> escapó y contamina el río.
/// Requiere Rigidbody (3D) y un Collider (3D) marcado como "Is Trigger".
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Waste : MonoBehaviour
{
    [Header("Dirección del flujo (hacia el jugador)")]
    public Vector3 flowDirection = Vector3.back; // -Z (hacia la cámara)

    private float speed;
    private bool counted = false; // evita contar dos veces

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // lo movemos manualmente, sin caer
        rb.constraints = RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezeRotation;

        // La velocidad depende del nivel actual (más nivel = más rápido).
        speed = (GameManager.Instance != null)
            ? GameManager.Instance.CurrentWasteSpeed()
            : 2f;
    }

    void Update()
    {
        if (GameManager.Instance != null &&
           (GameManager.Instance.isPaused || GameManager.Instance.isGameOver))
            return;

        transform.Translate(flowDirection.normalized * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (counted) return;

        if (other.CompareTag("Player"))
        {
            counted = true;
            if (GameManager.Instance != null) GameManager.Instance.OnWasteCaught();
            Destroy(gameObject);
        }
        else if (other.CompareTag("DeadZone"))
        {
            counted = true;
            if (GameManager.Instance != null) GameManager.Instance.OnWasteMissed();
            Destroy(gameObject);
        }
    }
}
