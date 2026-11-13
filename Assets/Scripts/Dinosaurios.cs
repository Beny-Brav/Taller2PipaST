using UnityEngine;

public class Dinosaurio : MonoBehaviour
{
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private float duracionMuerte = 1.5f;

    private Animator animator;
    private bool estaMuerto = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (estaMuerto) return;

        transform.Translate(
            Vector3.back * velocidad * Time.deltaTime,
            Space.World
        );
    }

    public void Morir()
    {
        if (estaMuerto) return;

        estaMuerto = true;

        if (animator != null)
        {
            animator.SetTrigger("Morir");
        }

        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        Destroy(gameObject, duracionMuerte);
    }
}
