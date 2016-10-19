// original source:
// http://www.xbdev.net/physics/Verlet/index.php

using UnityEngine;
using System.Collections;
using Bowk;

public class Verlet : MonoBehaviour
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

	[System.Serializable]
	public struct ConstraintPosition
	{
		public int index_0;
		public int index_1;
		public float rest_length;
	};

	[System.Serializable]
	public struct ConstraintSoft
	{
		public int index_0;
		public int index_parent;
		public Vector3 target_offset;

		public float min_dist;
		public float max_dist;
	};

	[System.Serializable]
	public struct Point
	{
		public Vector3 curr_pos;
		public bool	is_fixed;

		public Matrix4x4 curr_mat;
		public Matrix4x4 prev_mat;
	};

	public bool damp_enabled = true;
	public bool gravity_enabled = false;

	private const float damp = 0.1f;
	private const float ang_scale = 50f;
	private const float _gravity = -9.8f;

	public InitPoint[] init_data;

    public Point[] _points;
	public ConstraintPosition[]  _pos_constraints;
	public ConstraintSoft[]  _soft_constraints;

	void Start()
	{
		// init from init data
		//--------------------------------------------------
		_points = new Point[init_data.Length];
		for(int i = 0; i < init_data.Length; ++i)
		{
			if (init_data[i].parent < 0) init_data[i].is_fixed = true;

			_points[i] = new Point();
			_points[i].curr_pos = init_data[i].pos;
			_points[i].is_fixed = init_data[i].is_fixed;
		}
			
		BetterList<ConstraintPosition> new_const_pos = new BetterList<ConstraintPosition>();
		for(int i = 0; i < init_data.Length; ++i)
		{
			if (init_data[i].parent >= 0)
			{
				ConstraintPosition cp = new ConstraintPosition();
				cp.index_0 = i;
				cp.index_1 = init_data[i].parent;
				new_const_pos.Add(cp);
			}
		}
		_pos_constraints = new ConstraintPosition[0];
		if (new_const_pos.size > 0) _pos_constraints = new_const_pos.ToArray();

		BetterList<ConstraintSoft> new_const_soft = new BetterList<ConstraintSoft>();
		for(int i = 0; i < init_data.Length; ++i)
		{
			if (init_data[i].soft_clamp)
			{
				ConstraintSoft cs = new ConstraintSoft();
				cs.index_0 = i;
				cs.index_parent = init_data[i].parent;
				cs.min_dist = init_data[i].soft_min;
				cs.max_dist = init_data[i].soft_max;
				new_const_soft.Add(cs);
			}
		}
		_soft_constraints = new ConstraintSoft[0];
		if (new_const_soft.size > 0) _soft_constraints = new_const_soft.ToArray();

		//--------------------------------------------------

		for(int i = 0; i < _points.Length; ++i)
		{
			// set up matrix
			_points[i].curr_mat.SetTRS(_points[i].curr_pos, Quaternion.identity, Vector3.one);
			_points[i].prev_mat = _points[i].curr_mat;
		}

		for(int i = 0; i < _pos_constraints.Length; ++i)
		{
			Vector3 p0 = _points[_pos_constraints[i].index_0].curr_mat.GetColumn(3);
			Vector3 p1 = _points[_pos_constraints[i].index_1].curr_mat.GetColumn(3);

			float d = Vector3.Distance(p0, p1);
			_pos_constraints[i].rest_length = d;
		}

		for(int i = 0; i < _soft_constraints.Length; ++i)
		{
			Vector3 p0 = _points[_soft_constraints[i].index_0].curr_mat.GetColumn(3);
			Vector3 p1 = _points[_soft_constraints[i].index_parent].curr_mat.GetColumn(3);

			Vector3 offset = p0-p1;

			_soft_constraints[i].target_offset = offset;
		}
	}

	void Update()
	{			
        VerletIntegrate(Time.deltaTime);

		SatisfyConstraints();

		DebugDraw();
	}
	
  	void VerletIntegrate(float dt)
	{
		const float MAX_DT = 0.1f;
		dt = Mathf.Min(dt, MAX_DT);

		FillAngleConstraintBuffer();

		for (int i = 0; i < _points.Length; i++)
		{
			if (_points[i].is_fixed) continue;

			Vector3 oldPos = _points[i].prev_mat.MultiplyPoint(Vector3.zero);
			Vector3 curPos = _points[i].curr_mat.MultiplyPoint(Vector3.zero);
			Vector3 a = Vector3.zero;

			const float str = 15f;
			a.x += Input.GetAxis("Horizontal") * str;
			a.z += Input.GetAxis("Vertical") * str;
			if (Input.GetKey(KeyCode.E)) a.y += 1f * str;
			if (Input.GetKey(KeyCode.Q)) a.y -= 1f * str;

			// add angle contraint force
			a += _angle_constraint_force[i] * ang_scale;

			if (gravity_enabled) a += new Vector3(0, _gravity, 0f);

			//Vector3 tp = transform.position;
			//Debug.DrawLine(tp + _points[i].curr_pos, tp + _points[i].curr_pos + a, Color.magenta);

			if (damp_enabled)
			{
				float max = 2f - damp;
				float min = 1f - damp;
				curPos = (max*curPos) - (min*oldPos) + (a*dt*dt);
			}
			else
			{
				curPos += curPos - oldPos + (a*dt*dt);
			}

			_points[i].prev_mat = _points[i].curr_mat;
			_points[i].curr_mat.SetColumn(3, new Vector4(curPos.x, curPos.y, curPos.z, 1f));
		}
	}

	void SatisfyConstraints()
	{
		const int numIterations = 5;

		for (int i = 0; i < numIterations; i++)
		{
			for (int k = 0; k < _pos_constraints.Length; k++)
			{
				ConstraintPosition c = _pos_constraints[k];

				// Constraint (floor)
				for (int v = 0; v < _points.Length; v++)
				{
					Vector3 cp = _points[v].curr_mat.GetColumn(3);
					if (cp.y + transform.position.y < 0f)
					{
						_points[v].curr_mat.SetColumn(3, new Vector4(cp.x, -transform.position.y, cp.z, 1f));
					}
				}
					
				// positions constraint
				Vector3 p0 = _points[c.index_0].curr_mat.GetColumn(3);
				Vector3 p1 = _points[c.index_1].curr_mat.GetColumn(3);
				Vector3 delta = p1-p0;

				float len = delta.magnitude;


				float diff = (len - c.rest_length) / len;
				//p0 += delta * 0.5f * diff;
				//p1 -= delta * 0.5f * diff;

				// mass change
				// push child more than parent 
				p0 += delta * 0.8f * diff;
				p1 -= delta * 0.2f * diff;

				_points[c.index_0].curr_mat.SetColumn(3, new Vector4(p0.x, p0.y, p0.z, 1f));
				_points[c.index_1].curr_mat.SetColumn(3, new Vector4(p1.x, p1.y, p1.z, 1f));


				if (_points[c.index_0].is_fixed) _points[c.index_0].curr_mat = _points[c.index_0].prev_mat;
				if (_points[c.index_1].is_fixed) _points[c.index_1].curr_mat = _points[c.index_1].prev_mat;
			}

		}
	}

	BetterList<Vector3> _angle_constraint_force = new BetterList<Vector3>();
	void FillAngleConstraintBuffer()
	{
		_angle_constraint_force.Clear();
		for(int i = 0; i < _points.Length; ++i)
		{
			_angle_constraint_force.Add(Vector3.zero);
		}

		for (int a = 0; a < _soft_constraints.Length; a++)
		{
			ConstraintSoft con = _soft_constraints[a];

			Vector3 p0 = _points[con.index_0].curr_mat.GetColumn(3);
			Vector3 p1 = _points[con.index_parent].curr_mat.GetColumn(3);

			Vector3 force = Vector3.zero;

			Vector3 curr_offset = p0-p1;
			Vector3 target = _points[con.index_parent].curr_mat.inverse.MultiplyVector(_soft_constraints[a].target_offset);

			Vector3 diff = target - curr_offset;
			float dist = diff.magnitude;

			float multi = 0f;
			if (dist > con.min_dist)
			{
				multi = (dist-con.min_dist) / Mathf.Max(0.01f, con.max_dist);
			}

			force = diff * multi;

			//---

			_angle_constraint_force[con.index_0] += force;

			//Vector3 tp = transform.position;
			//Debug.DrawLine(tp + p0, tp + p0 + force, Color.yellow);
			//Debug.DrawLine(tp + p1, tp + p1 + target, Color.cyan);
		}
	}

	private void DebugDraw()
	{
		bool DEBUG_MATRICES = false;

		Vector3 tp = transform.position;

		// debug matrices
		for(int i = 0; DEBUG_MATRICES && i < _points.Length; ++i)
		{
			Matrix4x4 m = _points[i].curr_mat;

			const float s = 0.4f;
			Vector3 p = m.GetColumn(3);
			Vector3 r = m.GetRow(0) * s;
			Vector3 u = m.GetRow(1) * s;
			Vector3 f = m.GetRow(2) * s;

			Debug.DrawLine(tp + p, tp + p + r, Color.red);
			Debug.DrawLine(tp + p, tp + p + u, Color.green);
			Debug.DrawLine(tp + p, tp + p + f, Color.blue);
		}
	}

	//-----------

	void OnDrawGizmos()
	{
		Vector3 offset = transform.position;

		// Points
		for(int i = 0; _points != null && i < _points.Length; ++i)
		{
			Gizmos.color = Color.red;
			if (_points[i].is_fixed) Gizmos.color = Color.grey;

			Vector3 p = _points[i].curr_mat.GetColumn(3);

			Gizmos.DrawCube(offset + p, Vector3.one * 0.2f);
			//Gizmos.DrawWireCube(offset + _points[i].curr_pos, Vector3.one);
			UtilGizmos.DrawCircleGizmo(offset + p, 0.04f);
		}
		
		// position constraints
		Gizmos.color = Color.black;
		for(int i = 0; _pos_constraints != null && i < _pos_constraints.Length; ++i)
		{
			int i0 = _pos_constraints[i].index_0;
			int i1 = _pos_constraints[i].index_1;

			Vector3 p0 = _points[i0].curr_mat.GetColumn(3);
			Vector3 p1 = _points[i1].curr_mat.GetColumn(3);

			Vector3 pos0 = offset + p0;
			Vector3 pos1 = offset + p1;

			Gizmos.DrawLine(pos0, pos1);
		}

		// angle constraints

		Gizmos.color = Color.white;
	}

}



































