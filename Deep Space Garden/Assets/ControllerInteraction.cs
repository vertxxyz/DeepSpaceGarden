using UnityEngine;
using System.Collections;

public class ControllerInteraction : MonoBehaviour {
	SteamVR_TrackedController controller;
	int interactiveLayer;
	int nonInteractiveLayer;

	Vector3 originalPosition;
	Rigidbody rB;

	// Use this for initialization
	void Start () {
		controller = transform.GetComponentInParent<SteamVR_TrackedController> ();
		controller.TriggerClicked += UseTrigger;
		controller.TriggerUnclicked += UnuseTrigger;
		interactiveLayer = LayerMask.NameToLayer ("Controllers");
		nonInteractiveLayer = LayerMask.NameToLayer ("ControllersInactive");
		originalPosition = transform.localPosition;
		rB = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		rB.AddForce ((transform.parent.TransformPoint (originalPosition) - transform.position) / Time.fixedDeltaTime, ForceMode.VelocityChange);
		//rB.velocity = (transform.parent.TransformPoint (originalPosition) - transform.position) / Time.fixedDeltaTime;
	}


	void UseTrigger (object sender, ClickedEventArgs args) {
		gameObject.layer = interactiveLayer;
	}

	void UnuseTrigger (object sender, ClickedEventArgs args) {
		gameObject.layer = nonInteractiveLayer;
	}
}
