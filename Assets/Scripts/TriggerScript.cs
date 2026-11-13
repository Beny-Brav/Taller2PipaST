using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject readSection;
    [SerializeField] private int Movimiento = 70; 
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Instantiate(readSection, new Vector3(0, 0, Movimiento), Quaternion.identity);
        }
    }
}
