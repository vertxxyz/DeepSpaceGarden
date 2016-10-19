using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bowk;

public class PlantManager : MonoSingleton<PlantManager>
{

	public List<PlantAvoid> _avoids = null;

	public override void Init()
	{
		_avoids = new List<PlantAvoid>();
	}

	public void AddAvoid(PlantAvoid pa)
	{
		if(!_avoids.Contains(pa)) _avoids.Add(pa);
	}

	public void RemoveAvoid(PlantAvoid pa)
	{
		if(_avoids.Contains(pa)) _avoids.Remove(pa);
	}

}
