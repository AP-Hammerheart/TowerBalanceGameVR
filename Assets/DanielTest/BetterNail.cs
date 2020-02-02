using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BetterNail : MonoBehaviour
{
	//SoundStuff
	public AK.Wwise.Event Flyingnail;
	public AK.Wwise.Event BreakEv;
	//public AK.Wwise.Event HammerHit;
	public AK.Wwise.Event NailStickEv;

	public float hitMove = 0f;
	public float startOffset = 0.1f;
	public float overlapOffset = 0.1f;
	public float overlapRadius = 0.1f;
	public float breakForce = 1000;
	public float breakTorque = 1000;

	bool isFlying;
	float flyingTimer;
	Vector3 velocity;
	Rigidbody rigidBody;
	List<Joint> joints = new List<Joint>();

	public void Fly(Vector3 velocity)
	{
		isFlying = true;
		this.velocity = velocity;
		prevFlying = transform.position;
	}

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.isKinematic = true;

		if (!isFlying)
		{
				StickNailToSomething();
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position - transform.up * startOffset * transform.lossyScale.x, overlapRadius * transform.lossyScale.x);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position - transform.up * overlapOffset * transform.lossyScale.x, overlapRadius * transform.lossyScale.x);
	}

	void OnJointBreak(float breakForce)
	{
		Debug.Log("JOINT BREAK");
		Break();
	}

	private void StickNailToSomething()
	{
		var colliders = Physics.OverlapSphere(transform.position - transform.up * overlapOffset * transform.lossyScale.x, overlapRadius * transform.lossyScale.x);
		var others = colliders.Where(
			c => c.attachedRigidbody != null 
			&& c.attachedRigidbody != rigidBody 
			&& c.GetComponent<NailShooter>() == null
			&& c.GetComponent<BetterNail>() == null)
			.ToList();

		rigidBody.isKinematic = false;

		NailStickEv.Post(gameObject);

		foreach (var other in others)
		{
			Debug.Log("Gonna stick this nail to " + other.gameObject.name);

			var joint = gameObject.AddComponent<FixedJoint>();
			//joint.connectedBody = other.attachedRigidbody;
			//var joint = gameObject.AddComponent<HingeJoint>();
			joint.connectedBody = other.attachedRigidbody;
			joint.anchor = gameObject.transform.InverseTransformPoint(transform.position - transform.up * overlapOffset * transform.lossyScale.x);

			joint.breakForce = breakForce;
			joint.breakTorque = breakTorque;

			joints.Add(joint);

			if (other.gameObject.GetComponent<BreakableObject>() == null )
			{
				other.gameObject.AddComponent<BreakableObject>();
				FindObjectOfType<LevelController>().breakableObjects.Add(other.gameObject.GetComponent<BreakableObject>());
			}

		}

		if (others.Count == 0)
		{
			Break();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			Break();
		}

		if (joints.Any(j => j.connectedBody == null))
		{
			Break();
		}

		if (isFlying)
		{
			flyingTimer += Time.deltaTime;

			velocity += Physics.gravity * Time.deltaTime;
			transform.position += velocity * Time.deltaTime;
			transform.rotation = Quaternion.LookRotation(velocity.normalized) * Quaternion.Euler(-90, 0, 0);

			var origin = prevFlying;
			var vec = transform.position - prevFlying;
			var ray = new Ray(origin, vec.normalized);
			if (Physics.Raycast(ray, out var hit, vec.magnitude, 0x7fffffff, QueryTriggerInteraction.Ignore))
			{
				var body = hit.rigidbody;
				if (body != null)
				{
					body.AddForce(velocity.normalized * 2f, ForceMode.Impulse);
					transform.position = hit.point - velocity.normalized * hitMove;
					StickNailToSomething();

					isFlying = false;
				}
			}
			else if (flyingTimer > 10f)
			{
				Destroy(this.gameObject);
			}

			prevFlying = transform.position;
		}
	}

	Vector3 prevFlying;

	bool isBreaking = false;
	private JointBreakListener breakListener;

	public void Break()
	{
		if (!isBreaking)
		{
			//SoundStuff
			BreakEv.Post(gameObject);
			Flyingnail.Post(gameObject);

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
		rigidBody.isKinematic = true;

		var joints = GetComponents<Joint>();
		foreach (var joint in joints)
		{
			Destroy(joint);
		}

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
}
