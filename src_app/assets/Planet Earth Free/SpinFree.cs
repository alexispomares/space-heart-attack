using UnityEngine;
using System.Collections;

/// <summary>
/// Spin the object at a specified speed
/// </summary>
public class SpinFree : MonoBehaviour {
	[Tooltip("Spin: Yes or No")]
	public bool spin;
	[Tooltip("Spin the parent object instead of the object this script is attached to")]
	public bool spinParent;
    public Rigidbody target;
    public float lateralSpeed;

    [HideInInspector]
    public float speed = 10f;
	[HideInInspector]
	public bool clockwise = true;
	[HideInInspector]
	public float direction = 1f;
	[HideInInspector]
	public float directionChangeSpeed = 2f;

    void Update() {

        if (target != null)
            speed = - target.velocity.z / 8;
        
		if (direction < 1f) {
			direction += Time.deltaTime / (directionChangeSpeed / 2);
		}

		if (spin) {
			if (clockwise) {
                if (spinParent)
                    transform.parent.transform.Rotate(Vector3.up, (speed * direction) * Time.deltaTime);
                else
                {
                    transform.Rotate(Vector3.right, -speed * direction * Time.deltaTime, Space.World);
                    transform.Rotate(Vector3.up, lateralSpeed * direction * Time.deltaTime, Space.World);
                }
            } else {
                if (spinParent)
                    transform.parent.transform.Rotate(-Vector3.up, (speed * direction) * Time.deltaTime);
                else
                {
                    transform.Rotate(Vector3.right, speed * direction * Time.deltaTime, Space.World);
                    transform.Rotate(Vector3.up, -lateralSpeed * direction * Time.deltaTime, Space.World);
                }
			}
		}
	}
}