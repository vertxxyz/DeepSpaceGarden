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

	public const float SCALE = 0.2f;

	//------------

	protected MeshFilter _mf = null;

	protected BetterList<Vector3> _verts = new BetterList<Vector3>();
	protected BetterList<int> _tris = new BetterList<int>();
	protected BetterList<Color> _colours = new BetterList<Color>();

	//------------

	void Awake()
	{
		_mf = GetComponent<MeshFilter>();
		_mf.mesh = new Mesh();
		_mf.mesh.MarkDynamic();
	}

	void OnEnable()
	{
		if (PlantManager.Exists) PlantManager.Instance.AddPlant(this);
	}

	void OnDisable()
	{
		if (PlantManager.Exists) PlantManager.Instance.RemovePlant(this);
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

		Vector3 scale = Vector3.one * SCALE;

		// points
		for(int i = 0; i < _verlet._points.Length; ++i)
		{
			Vector3 pos = _verlet._points[i].curr_mat.GetColumn(3);

			UtilShape.BuildCube(pos, Quaternion.identity, scale * 0.3f,
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

			UtilShape.BuildCylinder(p0, p1, SCALE * 0.2f, ref _verts, ref _tris);

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

		if (UnityEditor.EditorApplication.isPlaying) return;

		// position constraints
		Vector3 offset = transform.position;
		Gizmos.color = Color.red;
		for(int i = 0; init_data != null && i < init_data.Length; ++i)
		{
			int i0 = init_data[i].parent;
			if (i0 < 0 || i0 > init_data.Length) continue;

			Vector3 p0 = init_data[i].pos * SCALE;
			Vector3 p1 = init_data[i0].pos * SCALE;

			Vector3 pos0 = offset + p0;
			Vector3 pos1 = offset + p1;

			Gizmos.DrawLine(pos0, pos1);

			UtilGizmos.DrawCircleGizmo(pos0, 0.01f);
		}

	}
	#endif

}




































