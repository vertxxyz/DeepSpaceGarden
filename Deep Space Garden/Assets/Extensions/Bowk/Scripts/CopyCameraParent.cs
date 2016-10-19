using UnityEngine;
using System.Collections;

namespace Bowk
{

	[RequireComponent(typeof(Camera))]
	public class CopyCameraParent : MonoBehaviour
	{
		
		private Camera	_ParentCamera	= null;
		private Camera	_Camera			= null;
		
		void Awake()
		{
			_Camera = GetComponent<Camera>();
			
			_ParentCamera = transform.parent.GetComponent<Camera>();
			if (_ParentCamera == null)
			{
				Debug.LogWarning("No parent camera found");
				return;
			}
			
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			
			_Camera.orthographic = _ParentCamera.orthographic;
			_Camera.fieldOfView = _ParentCamera.fieldOfView;
		}
		
		void LateUpdate()
		{
			_Camera.orthographicSize = _ParentCamera.orthographicSize;
		}
	}

}