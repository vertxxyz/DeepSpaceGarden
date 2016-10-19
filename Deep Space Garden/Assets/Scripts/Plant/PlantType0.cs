using UnityEngine;
using System.Collections;
using Bowk;

// tall fuzzy

public class PlantType0 : Plant
{
	public float height = 2f;
	public int num_fuzz = 10;
	public float fuzz_dist = 2f;

	protected override void GeneratePlant()
	{
		BetterList<InitPoint> points = new BetterList<InitPoint>();

		InitPoint p = new InitPoint();
		p.pos = Vector3.zero;
		p.is_fixed = true;
		points.Add(p);

		p = new InitPoint();
		p.pos = new Vector3(0f, height, 0f);
		p.parent = 0;
		p.soft_min = 0.05f;
		p.soft_max = 0.05f;
		p.soft_clamp = true;
		points.Add(p);

		for(int i = 0; i < num_fuzz; ++i)
		{
			p = new InitPoint();
			Vector3 rnd = Random.insideUnitSphere * fuzz_dist;
			rnd += rnd.normalized * 0.2f;
			p.pos = new Vector3(0f, height, 0f) + rnd;

			p.parent = 1;
			p.soft_clamp = true;
			p.soft_min = 0.05f;
			p.soft_max = 0.1f;
			points.Add(p);
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
}
