using UnityEngine;
using System.Collections;

public class RotateLocalSimplified : MonoBehaviour {
	public float speed = 1;
	public Vector3 axis = Vector3.forward;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (axis * speed, Space.Self);
	}
}
