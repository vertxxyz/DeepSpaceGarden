using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bowk;

public class PlantManager : MonoSingleton<PlantManager>
{

	public List<PlantAvoid> _avoids = null;
	public List<Plant> _plants = null;

	public override void Init()
	{
		_avoids = new List<PlantAvoid>();
		_plants = new List<Plant>();
	}

	public void AddAvoid(PlantAvoid pa)
	{
		if(!_avoids.Contains(pa)) _avoids.Add(pa);
	}

	public void RemoveAvoid(PlantAvoid pa)
	{
		if(_avoids.Contains(pa)) _avoids.Remove(pa);
	}

	public void AddPlant(Plant p)
	{
		if(!_plants.Contains(p)) _plants.Add(p);
	}

	public void RemovePlant(Plant p)
	{
		if(_plants.Contains(p)) _plants.Remove(p);
	}

	public float DistanceToNearestPlant(Vector3 p)
	{
		float min_dist = float.MaxValue;

		for(int i = 0; i < _plants.Count; ++i)
		{
			float d = Vector3.Distance(_plants[i].transform.position, p);

			min_dist = Mathf.Min(min_dist, d);
		}

		return min_dist;
	}

}










