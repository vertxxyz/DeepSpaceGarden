using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LinearPositioner : MonoBehaviour {
	Transform _child;

	Transform child {
		get {
			if (_child == null) {
				if (transform.childCount > 0)
					_child = transform.GetChild (0);
			}
			return _child;
		}
	}

	public Transform positionRelativeTo;
	
	// Update is called once per frame
	void Update () {
		if (child == null || positionRelativeTo == null)
			return;
		Vector3 pos = Vector3.Project (positionRelativeTo.position - transform.position, transform.forward);
		child.localPosition = pos;
	}
}
