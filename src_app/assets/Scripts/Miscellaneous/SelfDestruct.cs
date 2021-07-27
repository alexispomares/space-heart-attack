using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

    public float lifetime;

    void Start ()
    {
        if (lifetime != 0)
            Destroy(gameObject, lifetime);
	}
	
    void Update()
    {
        if (transform.childCount == 0)
            Destroy(gameObject);
    }

}
