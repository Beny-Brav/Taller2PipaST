using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject readSection;
    public Transform puntoSalida;

    private bool generado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !generado)
        {
            generado = true;

            GameObject nuevo = Instantiate(
                readSection,
                Vector3.zero,
                Quaternion.identity
            );

            Transform entradaNueva = nuevo.transform.Find("Entrada");

            if (entradaNueva != null)
            {
                Quaternion ajuste =
                    puntoSalida.rotation *
                    Quaternion.Inverse(entradaNueva.rotation);

                nuevo.transform.rotation = ajuste;

                nuevo.transform.position +=
                    puntoSalida.position - entradaNueva.position;
            }
        }
    }
}