using UnityEngine;
using System.Collections;

public class RotateLocal : MonoBehaviour {
	public RotateLocal masterRotator;

	public float radius = 1;
	[MinMax ("Circumference Speed", 0.001f, 1)]
	public float circumferenceSpeedMin = 0.1f;
	[HideInInspector]
	public float circumferenceSpeedMax = 10f;
	[MinMax ("Speed Change Time", 0.1f, 10)]
	public float speedChangeMin = 0.1f;
	[HideInInspector]
	public float speedChangeMax = 10f;
	public Vector3 axis = Vector3.forward;


	[ReadOnly]
	public float circumferenceSpeed;
	[ReadOnly]
	public float changeSpeedTimer = 0;
	private float changeSpeedTime;

	void Start () {
		ChangeSpeed ();
	}

	void ChangeSpeed () {
		changeSpeedTime = Random.Range (speedChangeMin, speedChangeMin);
		changeSpeedTimer = 0;
		circumferenceSpeed = Random.Range (circumferenceSpeedMin, circumferenceSpeedMax) * Mathf.Sign (Random.value - 0.5f);
	}

	
	// Update is called once per frame
	void Update () {
		if (masterRotator == null) {
			if (changeSpeedTimer > changeSpeedTime)
				ChangeSpeed ();
			RotateWithSpeed (circumferenceSpeed);
			changeSpeedTimer += Time.deltaTime;
		} else {
			RotateWithSpeed (masterRotator.circumferenceSpeed);
		}
	}

	void RotateWithSpeed (float c_speed) {
		float circumference = 2 * Mathf.PI * radius;
		transform.localRotation = transform.localRotation * Quaternion.AngleAxis (360 * (c_speed / circumference), axis);
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = Color.magenta;
		Bowk.UtilGizmos.DrawCircleGizmo (transform.position, radius, transform.up, transform.forward);
		Gizmos.color = Color.white;
	}
}
