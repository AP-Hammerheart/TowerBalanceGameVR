using UnityEngine;

public class NailHitter : MonoBehaviour
{
	float cooldown = 0f;

	private void OnTriggerEnter(Collider other)
	{
		cooldown -= Time.deltaTime;
		var nail = other.GetComponent<Nail>();

		if (nail != null && cooldown < 0f)
		{
			nail.Hit();
			cooldown = 0.2f;
		}
	}
}
