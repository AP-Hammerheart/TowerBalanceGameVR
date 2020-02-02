using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LowestPoint : MonoBehaviour
{

	public List<Transform> transforms;
	public float threshold;
	public LevelController cont;
	bool trig;

	float GetLowestPoint()
	{
		return transforms
			.Select(t => t.position.y)
			.Min();
	}

	private void Update()
	{
		if (GetLowestPoint() < threshold &&!trig)
		{
			trig = true;
			StartCoroutine(cont.GameOver());
		}
	}
}
