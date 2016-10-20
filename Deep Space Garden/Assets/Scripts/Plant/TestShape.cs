using UnityEngine;
using System.Collections;
using Bowk;

public class TestShape : MonoBehaviour
{

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

		//UtilShape.BuildSphere(Vector3.zero, Quaternion.identity, Vector3.one, 24, 16,
		//	ref _verts, ref _tris);

		for(int i = vc; i < _verts.size; ++i) _colours.Add(Color.white);
		vc = _verts.size;

		UtilShape.BuildSphere(new Vector3(0f, 0f, 0f), Quaternion.identity, Vector3.one * 1f,
			12, 8, ref _verts, ref _tris);

		for(int i = vc; i < _verts.size; ++i) _colours.Add(Color.cyan);
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

}




































