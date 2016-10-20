using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SteamVR_TrackedController))]
public class TeleportController : MonoBehaviour {

	SteamVR_TrackedController controller;
	TeleportAreaController playAreaController;

	void Start () {
		controller = GetComponent<SteamVR_TrackedController> ();
		playAreaController = GameObject.FindObjectOfType<TeleportAreaController> ();
		controller.PadClicked += PadButtonPressed;
		controller.PadUnclicked += PadButtonRelease;
	}

	bool GetHit (out Vector3 pos) {
		RaycastHit hit;
		LayerMask mask = int.MaxValue;
		mask.RemoveFromMask ("Controllers");
		if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity, mask)) {
			NavMeshHit navHit;
			if (NavMesh.SamplePosition (hit.point, out navHit, 10, NavMesh.AllAreas)) {
				//Debug.DrawLine (transform.position, navHit.position, Color.cyan, 10);
				pos = navHit.position;
				return true;
			}
		}
		pos = Vector3.zero;
		return false;
	}

	void PadButtonPressed (object senderObj, ClickedEventArgs arguments) {
		StopAllCoroutines ();
		StartCoroutine (PadButtonPressing ());
	}

	IEnumerator PadButtonPressing () {
		while (true) {
			Vector3 p;
			if (GetHit (out p)) {
				playAreaController.EnableAndMoveTo (p);
			} else {
				playAreaController.Disable ();
			}
			yield return null;
		}
	}

	void PadButtonRelease (object senderObj, ClickedEventArgs arguments) {
		StopAllCoroutines ();
		Vector3 p;
		if (GetHit (out p)) {
			playAreaController.DisableAndTeleport (p);
		} else {
			playAreaController.Disable ();
		}
	}
}
