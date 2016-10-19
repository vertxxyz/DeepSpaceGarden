using UnityEngine;
using System.Collections;
using Bowk;

public class Plant : MonoBehaviour
{
	[System.Serializable]
	public struct InitPoint
	{
		public Vector3 pos;
		public int parent;
		public bool is_fixed;

		public bool soft_clamp;
		public float soft_min;
		public float soft_max;
	}
	public InitPoint[] init_data;

	protected Verlet _verlet = null;

	void Start()
	{
		_verlet = new Verlet(init_data, this.transform);
	}

	void Update()
	{			
		_verlet.Update();
	}

	//-----------

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (_verlet != null) _verlet.DrawGizmos();
	}
	#endif

}




































