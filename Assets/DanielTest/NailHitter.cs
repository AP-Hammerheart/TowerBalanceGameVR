using UnityEngine;

public class NailHitter : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		var nail = other.GetComponent<Nail>();

		if (nail != null)
		{
			nail.Hit();
		}
	}
}
