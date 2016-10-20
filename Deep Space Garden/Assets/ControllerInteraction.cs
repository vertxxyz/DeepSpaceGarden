using UnityEngine;
using System.Collections;

public class ControllerInteraction : MonoBehaviour {
	SteamVR_TrackedController controller;
	int interactiveLayer;
	int nonInteractiveLayer;

	// Use this for initialization
	void Start () {
		controller = transform.GetComponentInParent<SteamVR_TrackedController> ();
		controller.TriggerClicked += UseTrigger;
		controller.TriggerUnclicked += UnuseTrigger;
		interactiveLayer = LayerMask.NameToLayer ("Controllers");
		nonInteractiveLayer = LayerMask.NameToLayer ("ControllersInactive");
	}

	void UseTrigger (object sender, ClickedEventArgs args) {
		gameObject.layer = interactiveLayer;
		Debug.Log ("Clicked trigger");
	}

	void UnuseTrigger (object sender, ClickedEventArgs args) {
		gameObject.layer = nonInteractiveLayer;
		Debug.Log ("UnClicked trigger");
	}
}
