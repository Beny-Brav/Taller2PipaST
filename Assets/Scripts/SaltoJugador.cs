using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SaltoJugador : MonoBehaviour
{
    // Fuerza de salto del jugador
    [SerializeField] private float fuerzaSalto = 8f;

    // Componente Rigidbody del jugador
    private Rigidbody cuerpo;

    // Indica si el jugador esta en el suelo
    private bool estaEnSuelo = false;

    // Indica si el jugador ha solicitado un salto
    private bool saltoPendiente = false;

    private void Awake()
    {
        cuerpo = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Saltar cuando esta en el suelo el jugador y se presiona el espacio
        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo)
        {
            saltoPendiente = true;
            estaEnSuelo = false;
        }
    }

    private void FixedUpdate()
    {
        if (saltoPendiente)
        {
            // Reiniciar velocidad vertical antes del salto
            Vector3 velocidad = cuerpo.velocity;
            velocidad.y = 0f;
            cuerpo.velocity = velocidad;

            // Fuerza de salto aplicada al jugador
            cuerpo.AddForce(
                Vector3.up * fuerzaSalto,
                ForceMode.Impulse
            );

            saltoPendiente = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ComprobarSuelo(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        ComprobarSuelo(collision);
    }
    private void ComprobarSuelo(Collision collision)
    {
        if (cuerpo.velocity.y > 0.1f)
            return;

        foreach (ContactPoint contacto in collision.contacts)
        {
            if (contacto.normal.y > 0.5f)
            {
                estaEnSuelo = true;
                return;
            }
        }
    }
}

