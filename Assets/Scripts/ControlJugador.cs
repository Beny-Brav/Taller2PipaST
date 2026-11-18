
using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento = 6f;
    [SerializeField] private float distanciaMaxima = 4f;

    [SerializeField] private GameObject proyectil;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float intervaloDisparo = 0.5f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoDisparo;

    private float posicionInicialX;
    private float siguienteDisparo;

    private void Start()
    {
        posicionInicialX = transform.position.x;
    }

    private void Update()
    {
        float movimiento = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            movimiento -= 1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movimiento += 1f;
        }

        float nuevaX = transform.position.x +
                       movimiento * velocidadMovimiento * Time.deltaTime;

        float limiteIzquierdo = posicionInicialX - distanciaMaxima;
        float limiteDerecho = posicionInicialX + distanciaMaxima;

        nuevaX = Mathf.Clamp(nuevaX, limiteIzquierdo, limiteDerecho);

        transform.position = new Vector3(
            nuevaX,
            transform.position.y,
            transform.position.z
        );

        if (Input.GetKey(KeyCode.F) && Time.time >= siguienteDisparo)
        {
            Disparar();
        }
    }

    private void Disparar()
    {
        if (proyectil == null || puntoDisparo == null)
            return;

        Instantiate(proyectil, puntoDisparo.position, Quaternion.identity);

        if (audioSource != null && sonidoDisparo != null)
        audioSource.PlayOneShot(sonidoDisparo);

        siguienteDisparo = Time.time + Mathf.Max(0.5f, intervaloDisparo);
    }
}
