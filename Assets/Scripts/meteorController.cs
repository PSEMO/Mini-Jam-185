using Mono.Cecil.Cil;
using UnityEngine;

public class meteorController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Player"))
        {
            Debug.Log(gameObject.name + " " + other.gameObject.name);
            Destroy(gameObject);
        }
    }
}
