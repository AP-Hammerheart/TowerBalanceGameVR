using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Nail : MonoBehaviour
{
	public int numberOfHits = 3;
	public float hitMove = 0f;
	public float startOffset = 0.1f;
	public float overlapOffset = 0.1f;
	public float overlapRadius = 0.1f;
	public float breakForce = 1000;
	public float breakTorque = 1000;

	Rigidbody sourceObject;
	int hitCount = 0;

	private void Start()
	{
		if (sourceObject == null)
		{
			var colliders = Physics.OverlapSphere(transform.position - transform.up * startOffset, overlapRadius);
			var body = colliders.Select(c => c.attachedRigidbody).FirstOrDefault(b => b != null);
			if (body != null)
			{
				AttachToObject(body);
				hitCount = 100;
				StickNailToSomething();
			}
		}
	}

	public void AttachToObject(Rigidbody sourceObject)
	{
		this.sourceObject = sourceObject;
		transform.SetParent(sourceObject.transform, true);
	}

	public void Hit()
	{
		if (hitCount < numberOfHits)
		{
			hitCount++;
			transform.position += transform.up * hitMove;

			if (hitCount == numberOfHits)
			{
				StickNailToSomething();
			}
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position - transform.up * startOffset, overlapRadius);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position - transform.up * overlapOffset, overlapRadius);
	}

	private void StickNailToSomething()
	{
		var colliders = Physics.OverlapSphere(transform.position - transform.up * overlapOffset, overlapRadius);
		var other = colliders.FirstOrDefault(c => c.attachedRigidbody != null && c.attachedRigidbody != sourceObject);

		if (other != null)
		{
			Debug.Log("Gonna stick this nail to " + other.gameObject.name);
			//var joint = sourceObject.gameObject.AddComponent<FixedJoint>();
			//joint.connectedBody = other.attachedRigidbody;
			var joint = sourceObject.gameObject.AddComponent<HingeJoint>();
			joint.connectedBody = other.attachedRigidbody;
			joint.anchor = sourceObject.transform.InverseTransformPoint(transform.position - transform.up * overlapOffset);

			joint.breakForce = breakForce;
			joint.breakTorque = breakTorque;

			breakListener = sourceObject.gameObject.AddComponent<JointBreakListener>();
			breakListener.JointBreak += Break;
		}
		else
		{
			Break();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			Hit();
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			Break();
		}
	}

	bool isBreaking = false;
	private JointBreakListener breakListener;

	public void Break()
	{
		if (!isBreaking)
		{
			if (breakListener != null)
			{
				breakListener.JointBreak -= Break;
				breakListener = null;
			}

			isBreaking = true;
			StartCoroutine(BreakSequence());
		}
	}

	IEnumerator BreakSequence()
	{
		var vel = transform.up * UnityEngine.Random.Range(2f, 5f)
			+ transform.right * UnityEngine.Random.Range(-1f, 1f)
			+ transform.forward * UnityEngine.Random.Range(-1f, 1f);

		var rvel = UnityEngine.Random.onUnitSphere * 1900;

		transform.SetParent(null);

		for (float t = 0; t < 3f; t += Time.deltaTime)
		{
			yield return null;
			vel += Vector3.down * 15f * Time.deltaTime;
			transform.position += vel * Time.deltaTime;
			transform.rotation *= Quaternion.Euler(rvel * Time.deltaTime);
		}

		Destroy(this.gameObject);
	}

	public void DestroyJoint()
	{
		//if (joint != null)
		//{
		//	Destroy(joint);
		//	joint = null;
		//}
	}
}
