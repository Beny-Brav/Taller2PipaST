using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [SerializeField] private float velocidad = 25f;
    [SerializeField] private float tiempoVida = 5f;

    private void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.forward * velocidad * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Dinosaurio dinosaurio = other.GetComponentInParent<Dinosaurio>();

        if (dinosaurio != null)
        {
            dinosaurio.Morir();
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Obstaculo"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
