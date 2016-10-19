using UnityEngine;
using System.Collections;

// sphere that plants avoid

public class PlantAvoid : MonoBehaviour
{

	public float radius = 1f;

	void OnEnable()
	{
		PlantManager.Instance.AddAvoid(this);
	}

	void OnDisable()
	{
		PlantManager.Instance.RemoveAvoid(this);
	}

	#if UNITY_EDITOR

	void OnDrawGizmos()
	{
		if (!this.enabled) return;

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

	#endif


}
