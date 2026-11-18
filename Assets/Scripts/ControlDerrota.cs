using UnityEngine;
using UnityEngine.SceneManagement;


public class ControlDerrota : MonoBehaviour
{
    [SerializeField] private GameObject menuDerrota;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoMuerte;

    private bool perdio = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (menuDerrota != null)
            menuDerrota.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ComprobarDerrota(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        ComprobarDerrota(other);
    }

    private void ComprobarDerrota(Collider otro)
    {
        if (perdio) return;

        Dinosaurio dinosaurio = otro.GetComponentInParent<Dinosaurio>();

        bool esObstaculo = false;
        Transform objeto = otro.transform;

        while (objeto != null)
        {
            if (objeto.CompareTag("Obstaculo"))
            {
                esObstaculo = true;
                break;
            }

            objeto = objeto.parent;
        }

        if (dinosaurio != null || esObstaculo)
        {
            PerderPartida();
        }
    }

    private void PerderPartida()
    {
        perdio = true;

        if (menuDerrota != null)
            menuDerrota.SetActive(true);

        if (audioSource != null && sonidoMuerte != null)
        audioSource.PlayOneShot(sonidoMuerte);    

        Time.timeScale = 0f;
    }

    public void ReiniciarPartida()
    {
        Debug.Log("BOTON REINTENTAR PRESIONADO");
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}
