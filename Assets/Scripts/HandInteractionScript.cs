using UnityEngine;
using Valve.VR;

public class HandInteractionScript : HandInteractionScriptBase
{
	public SteamVR_Action_Boolean grabAction;
	public SteamVR_Action_Boolean useAction;
	private SteamVR_Behaviour_Pose pose;

	public override void Awake()
	{
		pose = GetComponent<SteamVR_Behaviour_Pose>();
		base.Awake();
	}

	public override void CheckInput()
	{
		if (grabAction.GetStateDown(pose.inputSource))
		{
			Grab();
		}
		if (grabAction.GetStateUp(pose.inputSource))
		{
			Release();
		}
		if (useAction != null && useAction.GetStateDown(pose.inputSource))
		{
			Use();
		}
	}

	public override Vector3 GetAngularVelocity()
	{
		return pose.GetAngularVelocity();
	}

	public override Vector3 GetVelocity()
	{
		return pose.GetVelocity();
	}
}