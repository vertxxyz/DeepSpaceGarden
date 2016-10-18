using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SteamVR_TrackedController))]
public class TeleportController : MonoBehaviour {

	SteamVR_TrackedController controller;

	void Start () {
		controller = GetComponent<SteamVR_TrackedController> ();
		controller.PadClicked += TrackpadButton;
	}

	
	void TrackpadButton (object senderObj, ClickedEventArgs arguments) {

		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity)) {
			NavMeshHit navHit;
			if (NavMesh.FindClosestEdge (hit.point, out navHit, NavMesh.AllAreas)) {

			}

		}
	}
}
