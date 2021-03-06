﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InteractableButton : MonoBehaviour {

	public Transform buttonTransform;

	public enum ButtonShape {
		circle,
		square,
	}

	[Header ("Shape Variables"), Space]
	public ButtonShape buttonShape;
	[EditorOnly]
	public float radius = 0.1f;
	[EditorOnly]
	public Vector2 size = new Vector2 (0.1f, 0.1f);



	[Header ("Button Variables"), Space]
	public float buttonThickness = 0.05f;
	public float depressionDistance = 0.04f;
	[Range (0.1f, 1)]
	public float activationDistance = 0.75f;
	[MinMax ("Force", 10, 5000)]
	public float minForce = 1;
	[HideInInspector]
	public float maxForce = 10;

	[ReadOnly, Space]
	public GameObject colliderGameObject;
	private Transform colliderTransform;
	private Rigidbody colliderRigidbody;

	[SerializeField, HideInInspector]
	private Mesh cylinderCollider;

	private Vector3 originalButtonPosition;

	[Space, Header ("Audio")]
	public AudioClip buttonDown;
	public AudioClip buttonPressedLoop;
	public AudioClip buttonUp;
	[ReadOnly]
	public OneShotSounds audioController;

	[Space, Header ("Events"), BetterUnityEvent]
	public UnityEvent onButtonDown;
	[BetterUnityEvent]
	public UnityEvent onButtonPressed;
	[BetterUnityEvent]
	public UnityEvent onButtonUp;

	// Use this for initialization
	void Start () {
		audioController = GameObject.FindObjectOfType<OneShotSounds> ();

		colliderGameObject = new GameObject (name + " Collider");
		colliderGameObject.layer = LayerMask.NameToLayer ("Interactables");
		colliderTransform = colliderGameObject.transform;
		colliderTransform.SetParent (transform);
		colliderTransform.localRotation = Quaternion.identity;
		colliderTransform.localPosition = Vector3.forward * buttonThickness / 2f;
		switch (buttonShape) {
		case ButtonShape.circle:
			MeshFilter mF = colliderGameObject.AddComponent<MeshFilter> ();
			mF.mesh = cylinderCollider;
			MeshCollider mC = colliderGameObject.AddComponent<MeshCollider> ();
			mC.convex = true;
			colliderTransform.localScale = new Vector3 (radius, radius, buttonThickness / 2f);
			break;
		case ButtonShape.square:
			BoxCollider bC = colliderGameObject.AddComponent<BoxCollider> ();
			bC.size = new Vector3 (size.x * 2, size.y * 2, buttonThickness);
			break;
		}
		colliderRigidbody = colliderGameObject.AddComponent<Rigidbody> ();
		colliderRigidbody.useGravity = false;
		colliderRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

		if (buttonTransform == null) {
			Debug.LogWarning ("Button Mesh Not Assigned on " + name);
		} else {
			buttonTransform.SetParent (transform);
			originalButtonPosition = buttonTransform.localPosition;
		}
	}

	void FixedUpdate () {
		ConstrainCollider ();
		ButtonPhysics ();
	}

	void ConstrainCollider () {
		Vector3 moveTo = transform.TransformPoint (new Vector3 (0, 0, Mathf.Max (depressionDistance / 2f, colliderTransform.localPosition.z)));
		colliderRigidbody.velocity = (moveTo - colliderTransform.position) / Time.fixedDeltaTime;
	}

	void ButtonPhysics () {
		float force = colliderTransform.localPosition.z - buttonThickness / 2f;
		colliderRigidbody.AddRelativeForce (-force * Vector3.forward * (minForce + Mathf.InverseLerp (0, buttonThickness / 2f, Mathf.Abs (force)) * maxForce), ForceMode.Force);
	}

	bool buttonDownBool = false;
	System.Action SoundCallback;

	// Update is called once per frame
	void Update () {
		if (buttonTransform == null)
			return;
		Vector3 worldDirection = transform.forward;
		Vector3 worldPosition = transform.TransformPoint (originalButtonPosition);
		buttonTransform.position = worldPosition + worldDirection * Mathf.Clamp (colliderTransform.localPosition.z - buttonThickness / 2f, 0, depressionDistance);
		float buttonDownNormalised = Mathf.InverseLerp (0, depressionDistance, colliderTransform.localPosition.z - buttonThickness / 2f);

		if (buttonDownNormalised >= activationDistance) {
			if (!buttonDownBool) {
				onButtonDown.Invoke ();
				buttonDownBool = true;
				if (audioController != null)
					audioController.PlaySound (buttonDown, transform.position);
			}
			if (audioController != null)
				SoundCallback = audioController.PlayLoopingSoundWithStopCallback (buttonPressedLoop, transform.position);
			onButtonPressed.Invoke ();
		} else if (buttonDownBool) {
			onButtonUp.Invoke ();
			if (audioController != null)
				audioController.PlaySound (buttonUp, transform.position);
			if (SoundCallback != null)
				SoundCallback.Invoke ();
			buttonDownBool = false;
		}
	}

	void OnDrawGizmosSelected () {
		switch (buttonShape) {
		case ButtonShape.circle:
			Gizmos.color = Color.cyan;
			Bowk.UtilGizmos.DrawCircleGizmo (transform.position, radius, transform.up, transform.forward);
			Gizmos.color = new Color (1, 0.5f, 0);
			Bowk.UtilGizmos.DrawCircleGizmo (transform.position + transform.forward * depressionDistance, radius, transform.up, transform.forward);
			Gizmos.color = Color.cyan;
			Bowk.UtilGizmos.DrawCircleGizmo (transform.position + transform.forward * buttonThickness, radius, transform.up, transform.forward);
			Gizmos.color = Color.green;
			Bowk.UtilGizmos.DrawCircleGizmo (transform.position + transform.forward * depressionDistance * activationDistance, radius, transform.up, transform.forward);
			Gizmos.color = Color.white;
			break;
		case ButtonShape.square:
			Gizmos.color = Color.cyan;
			Bowk.UtilGizmos.DrawSquareGizmo (transform.position, transform.up, transform.right, size);
			Gizmos.color = new Color (1, 0.5f, 0);
			Bowk.UtilGizmos.DrawSquareGizmo (transform.position + transform.forward * depressionDistance, transform.up, transform.right, size);
			Gizmos.color = Color.cyan;
			Bowk.UtilGizmos.DrawSquareGizmo (transform.position + transform.forward * buttonThickness, transform.up, transform.right, size);
			Gizmos.color = Color.green;
			Bowk.UtilGizmos.DrawSquareGizmo (transform.position + transform.forward * depressionDistance * activationDistance, transform.up, transform.right, size);
			Gizmos.color = Color.white;
			break;
		}
	}
}
