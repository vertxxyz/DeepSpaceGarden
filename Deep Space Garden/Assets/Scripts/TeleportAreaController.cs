﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SteamVR_PlayArea))]
public class TeleportAreaController : MonoBehaviour {
	SteamVR_PlayArea playArea;
	GameObject teleportPlayArea;
	[SerializeField, ReadOnly]
	Transform hmd;
	public GameObject playerTeleportLocationPrefab;
	private Transform playerTeleportLocation;
	// Use this for initialization
	void Start () {
		playArea = GetComponent<SteamVR_PlayArea> ();

		//Teleporting Play Area
		teleportPlayArea = new GameObject ("TeleportPlayArea");
		teleportPlayArea.transform.SetParent (transform);
		MeshRenderer mR = teleportPlayArea.AddComponent<MeshRenderer> ();
		MeshFilter mF = teleportPlayArea.AddComponent<MeshFilter> ();
		mF.mesh = playArea.GetComponent<MeshFilter> ().sharedMesh;
		mR.material = playArea.GetComponent<MeshRenderer> ().sharedMaterial;

		SteamVR_TrackedObject[] trackedObjects = transform.GetComponentsInChildren<SteamVR_TrackedObject> (true);
		for (int i = 0; i < trackedObjects.Length; i++) {
			if (trackedObjects [i].index == SteamVR_TrackedObject.EIndex.Hmd) {
				hmd = trackedObjects [i].transform.parent;
			}
		}

		playerTeleportLocation = GameObject.Instantiate (playerTeleportLocationPrefab).transform;
		playerTeleportLocation.SetParent (teleportPlayArea.transform);

		teleportPlayArea.SetActive (false);
	}

	Vector3 GetMoveTo (Vector3 position) {
		position -= new Vector3 (hmd.localPosition.x, 0, hmd.localPosition.z);
		Vector3 moveBy = Vector3.zero;
		for (int i = 0; i < playArea.vertices.Length / 2; i++) {
			Vector3 p = position + playArea.vertices [i];
			NavMeshHit hit;
			if (NavMesh.SamplePosition (p, out hit, Mathf.Infinity, NavMesh.AllAreas)) {
				moveBy = MaxMagnitudeEachComponent (moveBy, hit.position - p);
			}
		}
		return position + moveBy;
	}

	public void EnableAndMoveTo (Vector3 moveTo) {
		teleportPlayArea.SetActive (true);
		moveTo = GetMoveTo (moveTo);
		moveTo += Vector3.up * 0.01f;
		teleportPlayArea.transform.position = moveTo;
		StartCoroutine (SetPlayerPositionInTeleport ());
	}

	public void DisableAndTeleport (Vector3 moveTo) {
		Disable ();
		moveTo = GetMoveTo (moveTo);
		moveTo += Vector3.up * 0.015f;
		transform.position = moveTo;
	}

	IEnumerator SetPlayerPositionInTeleport () {
		while (true) {
			playerTeleportLocation.localPosition = hmd.transform.localPosition;
			playerTeleportLocation.localPosition = new Vector3 (playerTeleportLocation.localPosition.x, teleportPlayArea.transform.localPosition.y + 0.01f, playerTeleportLocation.localPosition.z);
			playerTeleportLocation.localScale = Vector3.one * Mathf.Lerp (.8f, 1.2f, (Mathf.Sin (Time.time * 3) + 1) / 2f);
			yield return null;
		}
	}

	public void Disable () {
		teleportPlayArea.SetActive (false);
		StopAllCoroutines ();
	}

	Vector3 MaxMagnitudeEachComponent (Vector3 one, Vector3 two) {
		Vector3 ret = one;
		if (Mathf.Abs (one.x) < Mathf.Abs (two.x))
			ret.x = two.x;
		if (Mathf.Abs (one.y) < Mathf.Abs (two.y))
			ret.y = two.y;
		if (Mathf.Abs (one.z) < Mathf.Abs (two.z))
			ret.z = two.z;
		return ret;
	}
}
