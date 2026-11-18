using UnityEngine;

public class Moneda : MonoBehaviour
{
    [SerializeField] private float velocidadGiro = 120f;
    [SerializeField] private AudioClip sonidoRecoleccion;

    [Header("Aparicion suave segun la distancia al jugador")]
    [SerializeField] private bool aparicionPorDistancia = true;
   
    [SerializeField] private float distanciaVisibleCompleta = 50f;
   
    [SerializeField] private float distanciaOculta = 90f;

    private static Transform jugador;

    private Vector3 escalaBase;
    private bool recolectada = false;

    private void Awake()
    {
       
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.isTrigger = true;
        }
    }

    private void Start()
    {
       
        escalaBase = transform.localScale;
    }

    private void Update()
    {
       
        transform.Rotate(Vector3.up, velocidadGiro * Time.deltaTime, Space.World);

        if (recolectada) return;

        if (jugador == null)
        {
            ControlJugador control = FindObjectOfType<ControlJugador>();

            if (control != null)
                jugador = control.transform;
        }

        if (jugador == null) return;

        if (aparicionPorDistancia)
        {
            float distancia = Vector3.Distance(transform.position, jugador.position);

          
            float t = Mathf.InverseLerp(distanciaOculta, distanciaVisibleCompleta, distancia);
            transform.localScale = escalaBase * Mathf.Max(t, 0.001f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (recolectada) return;

        if (other.GetComponentInParent<ControlJugador>() == null)
            return;

        recolectada = true;

        if (ControlPuntaje.Instancia != null)
            ControlPuntaje.Instancia.AgregarMoneda();

        if (sonidoRecoleccion != null)
        AudioSource.PlayClipAtPoint(sonidoRecoleccion, transform.position);    

        Destroy(gameObject);
    }
}