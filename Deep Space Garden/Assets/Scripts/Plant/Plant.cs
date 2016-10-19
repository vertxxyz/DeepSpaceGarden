using UnityEngine;
using System.Collections;
using Bowk;

public class Plant : MonoBehaviour
{
	public Color col_stem = Color.red;
	public Color col_joint = Color.magenta;

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

	//------------

	private MeshFilter _mf = null;

	private BetterList<Vector3> _verts = new BetterList<Vector3>();
	private BetterList<int> _tris = new BetterList<int>();
	private BetterList<Color> _colours = new BetterList<Color>();

	//------------

	void Awake()
	{
		_mf = GetComponent<MeshFilter>();
		_mf.mesh = new Mesh();
		_mf.mesh.MarkDynamic();
	}

	//------------

	protected virtual void GeneratePlant()
	{
	}

	protected virtual void Start()
	{
		GeneratePlant();

		_verlet = new Verlet(init_data, this.transform);
	}

	protected virtual void Update()
	{			
		_verlet.Update();
		BuildMesh();
	}

	protected virtual void BuildMesh()
	{
		_verts.Clear();
		_tris.Clear();
		_colours.Clear();

		//---

		int vc = _verts.size;

		// points
		for(int i = 0; i < _verlet._points.Length; ++i)
		{
			Vector3 pos = _verlet._points[i].curr_mat.GetColumn(3);

			UtilShape.BuildCube(pos, Quaternion.identity, Vector3.one * 0.3f,
				ref _verts, ref _tris);
		}
		for(int c = vc; c < _verts.size; ++c) _colours.Add(col_joint);
		vc = _verts.size;

		// connections
		for(int i = 0; i < _verlet._pos_constraints.Length; ++i)
		{
			Verlet.ConstraintPosition cp = _verlet._pos_constraints[i];
			Vector3 p0 = _verlet.GetPointPos(cp.index_0);
			Vector3 p1 = _verlet.GetPointPos(cp.index_1);

			UtilShape.BuildCylinder(p0, p1, 0.2f, ref _verts, ref _tris);

			Debug.DrawLine(transform.position + p0, transform.position + p1, Color.red);
		}
		for(int c = vc; c < _verts.size; ++c) _colours.Add(col_stem);
		vc = _verts.size;

		//---

		_mf.mesh.Clear();
		_mf.mesh.vertices = _verts.ToArray();
		_mf.mesh.triangles = _tris.ToArray();
		_mf.mesh.colors = _colours.ToArray();
		_mf.mesh.RecalculateBounds();
		_mf.mesh.RecalculateNormals();
	}

	//-----------

	#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{
		//if (_verlet != null) _verlet.DrawGizmos();
	}
	#endif

}




































