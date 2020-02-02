using System.Collections.Generic;
using UnityEngine;

public abstract class HandInteractionScriptBase : MonoBehaviour
{
	//SoundStuff
	public AK.Wwise.Event GrabEv;

	private FixedJoint joint;
	private Interactable currentInteractable;
	public List<Interactable> contactInteractables = new List<Interactable>();

	public virtual void Awake()
	{
		joint = GetComponent<FixedJoint>();
	}

	void Update()
	{
		CheckInput();
	}

	public abstract void CheckInput();

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Interactable"))
		{

			contactInteractables.Add(other.gameObject.GetComponentInParent<Interactable>());
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Interactable"))
		{
			contactInteractables.Remove(other.gameObject.GetComponentInParent<Interactable>());
		}
	}

	public void Use()
	{
		var usable = currentInteractable.GetComponent<Usable>();
		if (usable != null)
		{
			usable.Use();
		}
	}

	public void Grab()
	{
		currentInteractable = GetNearestInteractable();

		if (!currentInteractable)
		{
			return;
		}
		//SoundStuff
		GrabEv.Post(gameObject);

		if (currentInteractable.activeHand)
		{
			currentInteractable.activeHand.Release();
		}

		Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
		joint.connectedBody = targetBody;

		currentInteractable.activeHand = this;
	}

	public void Release()
	{
		if (!currentInteractable)
		{
			return;
		}

		Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
		targetBody.velocity = GetVelocity();
		targetBody.angularVelocity = GetAngularVelocity();

		joint.connectedBody = null;
		currentInteractable.activeHand = null;
		currentInteractable = null;

	}

	public abstract Vector3 GetAngularVelocity();
	public abstract Vector3 GetVelocity();

	private Interactable GetNearestInteractable()
	{
		Interactable nearest = null;
		float minDistance = float.MaxValue;
		float distance = 0.0f;

		foreach (Interactable inc in contactInteractables)
		{
			if (inc != null)
			{
				distance = (inc.transform.position - transform.position).sqrMagnitude;

				if (distance < minDistance)
				{
					minDistance = distance;
					nearest = inc;
				}
			}
		}

		return nearest;
	}
}