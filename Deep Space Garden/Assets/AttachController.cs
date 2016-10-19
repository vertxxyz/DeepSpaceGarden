using UnityEngine;
using System.Collections;

public class AttachController : MonoBehaviour {
	public GameObject prefab;
	// Use this for initialization
	void Start () {
		GameObject p = GameObject.Instantiate (prefab);
		p.transform.SetParent (transform);
	}
}
