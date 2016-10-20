using UnityEngine;
using System.Collections;

public class PlantSeed : MonoBehaviour
{
	public GameObject plant_prefab = null;

	public void Plant(Vector3 position, Transform parent)
	{
		// test if not close to other plants
		position.y = 0f;

		float min_dist = PlantManager.Instance.DistanceToNearestPlant(position);

		if (min_dist <= 0.5f) return;

		// create plant
		GameObject.Instantiate(plant_prefab, position, Quaternion.identity, parent);

		// destroy this seed
		Destroy(this.gameObject);
	}

}
