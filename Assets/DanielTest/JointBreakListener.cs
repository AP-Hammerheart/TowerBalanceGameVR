using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreakListener : MonoBehaviour
{
	public event Action JointBreak;

	private void OnJointBreak(float breakForce)
	{
		JointBreak?.Invoke();
	}
}
