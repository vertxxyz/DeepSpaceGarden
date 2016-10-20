using UnityEngine;
using System.Collections;
using Bowk;

// tall fuzzy

public class PlantType0 : Plant
{
	[MinMax("height", 0f, 10f)]
	public float min_height = 1f;
	[HideInInspector]
	public float max_height = 4f;

	[MinMax("fuzz", 1, 20)]
	public int min_fuzz = 5;
	[HideInInspector]
	public int max_fuzz = 15;

	[MinMax("fuzz_dist", 0.5f, 4f)]
	public float min_fuzz_dist = 1f;
	[HideInInspector]
	public float max_fuzz_dist = 4f;

	protected override void GeneratePlant()
	{
		BetterList<InitPoint> points = new BetterList<InitPoint>();

		InitPoint p = new InitPoint();
		p.pos = Vector3.zero;
		p.is_fixed = true;
		points.Add(p);

		float height = Random.Range(min_height, max_height);
		int num_fuzz = Random.Range(min_fuzz, max_fuzz);
		float fuzz_dist = Random.Range(min_fuzz_dist, max_fuzz_dist);

		p = new InitPoint();
		p.pos = new Vector3(0f, height, 0f);
		p.parent = 0;
		p.soft_min = 0.05f;
		p.soft_max = 0.05f;
		p.soft_clamp = true;
		points.Add(p);

		BetterList<Vector3> fuzz_points = new BetterList<Vector3>();
		BetterList<Vector3> fuzz_directions = new BetterList<Vector3>();

		fuzz_directions.Add(-Vector3.up);

		int attempt_count = 0;
		const int max_attempt = 1000;
		int fuzz_cout = 0;

		while (fuzz_cout < num_fuzz && attempt_count < max_attempt)
		{
			attempt_count++;

			p = new InitPoint();
			Vector3 rnd = Random.insideUnitSphere * fuzz_dist;
			rnd += rnd.normalized * 0.2f;
			p.pos = new Vector3(0f, height, 0f) + rnd;

			float min_angle = GetMinAngle(ref fuzz_directions, rnd);
			if (min_angle < 10f) continue;

			float min_dist = GetMinDist(ref fuzz_points, p.pos);
			if (min_dist < 0.5f) continue;

			p.parent = 1;
			p.soft_clamp = true;
			p.soft_min = 0.05f;
			p.soft_max = 0.1f;
			points.Add(p);
			fuzz_cout++;

			//fuzz_directions.Add(rnd);
			fuzz_points.Add(p.pos);
		}

		init_data = points.ToArray();
	}
		
	protected override void Start()
	{
		base.Start();
	}

	protected override void Update ()
	{
		base.Update();
	}

	protected override void BuildMesh()
	{
		base.BuildMesh ();
	}

	private float GetMinAngle(ref BetterList<Vector3> list, Vector3 d)
	{
		float min_angle = float.MaxValue;

		for(int i = 0; i < list.size; ++i)
		{
			min_angle = Mathf.Min(min_angle, Vector3.Angle(list[i], d));
		}

		return min_angle;
	}

	private float GetMinDist(ref BetterList<Vector3> list, Vector3 p)
	{
		float min_dist = float.MaxValue;

		for(int i = 0; i < list.size; ++i)
		{
			min_dist = Mathf.Min(min_dist, Vector3.Distance(list[i], p));
		}

		return min_dist;
	}

}
