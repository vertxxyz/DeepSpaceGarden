using UnityEngine;
using System.Collections;
using Bowk;

// slim line with multiple joints

public class PlantType1 : Plant
{
	[MinMax("link_height", 0.5f, 1.5f)]
	public float min_link_height = 1f;
	[HideInInspector]
	public float max_link_height = 4f;

	[MinMax("links", 3, 8)]
	public int min_links = 3;
	[HideInInspector]
	public int max_links = 4;

	[MinMax("offset", 0f, 2f)]
	public float min_offset = 5;
	[HideInInspector]
	public float max_offset = 15;

	protected override void GeneratePlant()
	{
		BetterList<InitPoint> points = new BetterList<InitPoint>();

		InitPoint p = new InitPoint();
		p.pos = Vector3.zero;
		p.is_fixed = true;
		points.Add(p);

		int num_links = Random.Range(min_links, max_links);

		int attempt_count = 0;
		const int max_attempt = 1000;
		int link_count = 0;

		while (link_count < num_links && attempt_count < max_attempt)
		{
			attempt_count++;

			InitPoint prev_point = points[link_count];

			p = new InitPoint();

			float link_height = Random.Range(min_link_height, max_link_height);

			float offset = Random.Range(min_offset, max_offset);
			Vector3 rnd = Random.insideUnitSphere * offset;

			p.pos = prev_point.pos + new Vector3(0f, link_height, 0f) + rnd;

			p.parent = link_count;

			p.soft_clamp = true;
			p.soft_min = 0f;
			p.soft_max = 0.1f;
			points.Add(p);
			link_count++;
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
