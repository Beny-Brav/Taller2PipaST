using UnityEngine;

public class GeneradorDinosaurios : MonoBehaviour
{
    [SerializeField] private GameObject[] dinosaurios;
    [SerializeField] private GameObject[] vehiculos;
    [SerializeField] private Transform[] puntosGeneracion;

    private void Start()
    {
        GenerarObjeto();
    }

    private void GenerarObjeto()
    {
        if (puntosGeneracion == null || puntosGeneracion.Length == 0)
            return;

        bool hayDinosaurios = dinosaurios != null && dinosaurios.Length > 0;
        bool hayVehiculos = vehiculos != null && vehiculos.Length > 0;

        if (!hayDinosaurios && !hayVehiculos)
            return;

        bool generarDinosaurio = hayDinosaurios &&
                                (!hayVehiculos || Random.value < 0.5f);

        GameObject[] opciones = generarDinosaurio ? dinosaurios : vehiculos;

        int tipo = Random.Range(0, opciones.Length);
        int carril = Random.Range(0, puntosGeneracion.Length);

        GameObject prefab = opciones[tipo];
        Transform punto = puntosGeneracion[carril];

        if (prefab == null || punto == null)
            return;

        GameObject nuevoObjeto = Instantiate(
            prefab,
            punto.position,
            prefab.transform.rotation
        );

        nuevoObjeto.transform.SetParent(transform, true);
    }
}