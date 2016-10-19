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

	public int directionMultiplier = 1;


	[ReadOnly]
	public float circumferenceSpeed;
	private float circumferenceSpeedTo;
	[ReadOnly]
	public float changeSpeedTimer = 0;
	private float changeSpeedTime;

	public AudioSource audioSource;
	[MinMax ("Pitch", 0.1f, 10)]
	public float pitchMin = 0.1f;
	[HideInInspector]
	public float pitchMax = 10f;

	private float circumference;

	void Start () {
		ChangeSpeed ();
		circumference = 2 * Mathf.PI * radius;
		;
	}

	void ChangeSpeed () {
		changeSpeedTime = Random.Range (speedChangeMin, speedChangeMin);
		changeSpeedTimer = 0;
		circumferenceSpeedTo = Random.Range (circumferenceSpeedMin, circumferenceSpeedMax) * Mathf.Sign (Random.value - 0.5f);
	}

	
	// Update is called once per frame
	void Update () {
		if (masterRotator == null) {
			if (changeSpeedTimer > changeSpeedTime)
				ChangeSpeed ();
			circumferenceSpeed = Mathf.MoveTowards (circumferenceSpeed, circumferenceSpeedTo, Time.deltaTime * circumferenceSpeedMax);
			RotateWithSpeed (circumferenceSpeed);
			if (Mathf.Approximately (circumferenceSpeed, circumferenceSpeedTo))
				changeSpeedTimer += Time.deltaTime;
		} else {
			RotateWithSpeed (masterRotator.circumferenceSpeed);
		}

		if (audioSource != null) {
			audioSource.pitch = Mathf.Clamp (circumferenceSpeed / circumference, pitchMin, pitchMax);
		}
	}

	void RotateWithSpeed (float c_speed) {
		c_speed *= directionMultiplier;
		transform.localRotation = transform.localRotation * Quaternion.AngleAxis (360 * (c_speed / circumference), axis);
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = Color.magenta;
		Bowk.UtilGizmos.DrawCircleGizmo (transform.position, radius, transform.up, transform.forward);
		Gizmos.color = Color.white;
	}
}
