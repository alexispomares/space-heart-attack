using UnityEngine;
using System.Collections;

public class DestroyTrigger : MonoBehaviour
{
    void OnTriggerEnter (Collider other)
    {
        if (!other.CompareTag("Player"))
            Destroy(other.gameObject);
	}
}
