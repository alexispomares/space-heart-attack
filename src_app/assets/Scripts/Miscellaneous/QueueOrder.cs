using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class QueueOrder : MonoBehaviour {

    public Material mat;
    public int order;

	void Start()
    {
        mat.renderQueue = order;
	}
}
