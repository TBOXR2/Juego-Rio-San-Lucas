using UnityEngine;

/// <summary>
/// (VERSIÓN 3D) Mueve al personaje SOLO en horizontal (eje X), lo mantiene
/// dentro de los límites, lo gira hacia donde camina y activa la animación.
/// Requiere un Rigidbody (3D) en el mismo objeto.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 6f;
    public float minX = -7f;   // límite izquierdo (ajústalo a tu escena)
    public float maxX = 7f;    // límite derecho

    [Header("Giro")]
    public bool rotarHaciaDireccion = true; // gira el modelo hacia donde camina

    private Rigidbody rb;
    private Animator animator;
    private float moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // El Animator suele estar en el modelo importado (hijo del objeto).
        animator = GetComponentInChildren<Animator>();

        rb.useGravity = false;
        // Solo se mueve en X y solo puede girar en Y (queda derecho y sin caer).
        rb.constraints = RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezePositionZ
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // -1 (izquierda), 0 (quieto), 1 (derecha). Funciona con flechas y A/D.
        moveInput = Input.GetAxisRaw("Horizontal");

        // Animación: parámetro booleano "isMoving" en el Animator.
        if (animator != null)
            animator.SetBool("isMoving", Mathf.Abs(moveInput) > 0.01f);

        // Gira al personaje hacia la dirección del movimiento.
        if (rotarHaciaDireccion && moveInput != 0)
        {
            Vector3 dir = new Vector3(moveInput, 0f, 0f);
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    void FixedUpdate()
    {
        Vector3 pos = rb.position;
        pos.x += moveInput * moveSpeed * Time.fixedDeltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        rb.MovePosition(pos);
    }
}
