using UnityEngine;
using System.Collections;

public class PlantBed : MonoBehaviour
{

	void OnTriggerEnter(Collider other)
	{
		PlantSeed ps = other.GetComponent<PlantSeed>();

		if (ps != null)
		{
			ps.Plant(ps.transform.position, this.transform);
		}
	}

	#if UNITY_EDITOR

	void OnDrawGizmos()
	{
		BoxCollider bc = GetComponent<BoxCollider>();

		if (bc != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(transform.position + bc.center, bc.size);
		}

	}

	#endif

}
