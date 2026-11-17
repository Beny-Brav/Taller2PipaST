using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GeneradorMonedas : MonoBehaviour
{
    [SerializeField] private GameObject monedaPrefab;
   
    [SerializeField] private Transform[] puntosGeneracion;

    [Header("Grupos de monedas por seccion")]
    [SerializeField] private int gruposMinimo = 2;
    [SerializeField] private int gruposMaximo = 4;
    [SerializeField] private int monedasPorGrupoMinimo = 3;
    [SerializeField] private int monedasPorGrupoMaximo = 6;
    [SerializeField] private float separacionMonedas = 2f;

    [Header("Zona donde pueden aparecer")]
   
    [SerializeField] private float largoZona = 60f;
   
    [SerializeField] private float alturaExtra = 0.5f;
   
    [SerializeField] private bool haciaElJugador = true;

    [Header("Depuracion")]
    [SerializeField] private bool mostrarMensajes = true;

    private struct Tramo
    {
        public int carril;
        public float inicio;
        public float fin;
    }

    private void Start()
    {
        if (monedaPrefab == null)
        {
            Debug.LogWarning("GeneradorMonedas: falta asignar 'monedaPrefab' en " + name, this);
            return;
        }

       
        if (!TieneAlgunPunto(puntosGeneracion))
        {
            puntosGeneracion = LeerPuntosDeDinosaurios();
        }

        if (!TieneAlgunPunto(puntosGeneracion))
        {
            Debug.LogWarning("GeneradorMonedas: no hay puntos de generacion en " + name
                             + ". Asigna 'puntosGeneracion' o pon este script junto a GeneradorDinosaurios.", this);
            return;
        }

        int totalMonedas = 0;

        int minGrupos = Mathf.Min(gruposMinimo, gruposMaximo);
        int maxGrupos = Mathf.Max(gruposMinimo, gruposMaximo);
        int grupos = Random.Range(minGrupos, maxGrupos + 1);

        if (grupos <= 0)
        {
            Debug.LogWarning("GeneradorMonedas: 'gruposMinimo' y 'gruposMaximo' valen 0 en " + name
                             + ". Pon, por ejemplo, 2 y 4.", this);
            return;
        }

        int minMonedas = Mathf.Max(1, Mathf.Min(monedasPorGrupoMinimo, monedasPorGrupoMaximo));
        int maxMonedas = Mathf.Max(minMonedas, Mathf.Max(monedasPorGrupoMinimo, monedasPorGrupoMaximo));

        List<Tramo> usados = new List<Tramo>();

        for (int g = 0; g < grupos; g++)
        {
            int cantidad = Random.Range(minMonedas, maxMonedas + 1);
            float largoGrupo = (cantidad - 1) * separacionMonedas;

           
            for (int intento = 0; intento < 10; intento++)
            {
                int carril = Random.Range(0, puntosGeneracion.Length);

                if (puntosGeneracion[carril] == null)
                    continue;

                float maxInicio = Mathf.Max(0f, largoZona - largoGrupo);
                float inicio = Random.Range(0f, maxInicio);
                float fin = inicio + largoGrupo;

                if (Choca(usados, carril, inicio, fin))
                    continue;

                usados.Add(new Tramo { carril = carril, inicio = inicio, fin = fin });
                CrearGrupo(puntosGeneracion[carril], inicio, cantidad);
                totalMonedas += cantidad;
                break;
            }
        }

        if (mostrarMensajes)
            Debug.Log("GeneradorMonedas (" + name + "): se generaron " + totalMonedas + " monedas", this);
    }

   
    private Transform[] LeerPuntosDeDinosaurios()
    {
        GeneradorDinosaurios dinosaurios = GetComponent<GeneradorDinosaurios>();

        if (dinosaurios == null)
            return null;

        FieldInfo campo = typeof(GeneradorDinosaurios).GetField(
            "puntosGeneracion",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
        );

        if (campo == null)
            return null;

        return campo.GetValue(dinosaurios) as Transform[];
    }

    private bool TieneAlgunPunto(Transform[] puntos)
    {
        if (puntos == null) return false;

        foreach (Transform punto in puntos)
        {
            if (punto != null)
                return true;
        }

        return false;
    }

    private bool Choca(List<Tramo> usados, int carril, float inicio, float fin)
    {
        float margen = separacionMonedas * 2f;

        foreach (Tramo t in usados)
        {
            if (t.carril != carril) continue;

            if (inicio < t.fin + margen && fin > t.inicio - margen)
                return true;
        }

        return false;
    }

    private void CrearGrupo(Transform punto, float inicioZ, int cantidad)
    {
        Vector3 direccion = haciaElJugador ? Vector3.back : Vector3.forward;

        for (int i = 0; i < cantidad; i++)
        {
            Vector3 posicion = punto.position
                             + direccion * (inicioZ + i * separacionMonedas)
                             + Vector3.up * alturaExtra;

            GameObject moneda = Instantiate(
                monedaPrefab,
                posicion,
                monedaPrefab.transform.rotation
            );

          
            moneda.transform.SetParent(transform, true);
        }
    }
}