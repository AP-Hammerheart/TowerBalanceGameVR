using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeHand : HandInteractionScriptBase
{
	public override void CheckInput()
	{
		if (Input.GetMouseButtonDown(2))
		{
			Use();
		}
	}

	public override Vector3 GetAngularVelocity()
	{
		return Vector3.zero;
	}

	public override Vector3 GetVelocity()
	{
		return Vector3.zero;
	}
}
