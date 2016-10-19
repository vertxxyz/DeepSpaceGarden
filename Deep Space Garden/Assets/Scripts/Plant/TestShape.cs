using UnityEngine;
using System.Collections;
using Bowk;

public class TestShape : MonoBehaviour
{

	public Vector3 start = Vector3.zero;
	public Vector3 end = Vector3.zero;
	public float girth = 0.2f;

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

	protected virtual void Update()
	{			
		BuildMesh();
	}

	protected virtual void BuildMesh()
	{
		_verts.Clear();
		_tris.Clear();
		_colours.Clear();

		//---

		int vc = _verts.size;

		UtilShape.BuildCylinder(start, end, girth,
			ref _verts, ref _tris);

		Debug.DrawLine(transform.position + start, transform.position + end, Color.red);

		UtilDebug.DrawCircle(transform.position + start, girth * 0.5f, Color.red, 0f,
			Vector3.forward, Vector3.up);
		UtilDebug.DrawCircle(transform.position + end, girth * 0.5f, Color.red, 0f,
			Vector3.forward, Vector3.up);

		//---

		_mf.mesh.Clear();
		_mf.mesh.vertices = _verts.ToArray();
		_mf.mesh.triangles = _tris.ToArray();
		_mf.mesh.colors = _colours.ToArray();
		_mf.mesh.RecalculateBounds();
		_mf.mesh.RecalculateNormals();
	}

	//-----------

}




































