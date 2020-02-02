using System.Collections;
using System.Linq;
using UnityEngine;

public class Nail : MonoBehaviour
{
	//SoundStuff
	public AK.Wwise.Event Flyingnail;
	public AK.Wwise.Event BreakEv;
	//public AK.Wwise.Event HammerHit;
	public AK.Wwise.Event NailStickEv;

	public int numberOfHits = 3;
	public float hitMove = 0f;
	public float startOffset = 0.1f;
	public float overlapOffset = 0.1f;
	public float overlapRadius = 0.1f;
	public float breakForce = 1000;
	public float breakTorque = 1000;

	Rigidbody sourceObject;
	int hitCount = 0;

	bool isFlying;
	float flyingTimer;
	Vector3 velocity;

	public void Fly(Vector3 velocity)
	{
		isFlying = true;
		this.velocity = velocity;
		prevFlying = transform.position;
	}

	private void Start()
	{
		if (sourceObject == null && !isFlying)
		{
			var colliders = Physics.OverlapSphere(transform.position - transform.up * startOffset * transform.lossyScale.x, overlapRadius * transform.lossyScale.x);
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
			bool Stick = false;
			if (hitCount == numberOfHits)
			{

				Stick = StickNailToSomething();
			}
			
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position - transform.up * startOffset * transform.lossyScale.x, overlapRadius * transform.lossyScale.x);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position - transform.up * overlapOffset * transform.lossyScale.x, overlapRadius * transform.lossyScale.x);
	}

	private bool StickNailToSomething()
	{
		var colliders = Physics.OverlapSphere(transform.position - transform.up * overlapOffset * transform.lossyScale.x, overlapRadius * transform.lossyScale.x);
		var other = colliders.FirstOrDefault(c => c.attachedRigidbody != null && c.attachedRigidbody != sourceObject && c.GetComponent<NailHitter>() == null);

		NailStickEv.Post(gameObject);

		if (other != null)
		{


			


			Debug.Log("Gonna stick this nail to " + other.gameObject.name);
			var joint = sourceObject.gameObject.AddComponent<FixedJoint>();
			//joint.connectedBody = other.attachedRigidbody;
			//var joint = sourceObject.gameObject.AddComponent<HingeJoint>();
			joint.connectedBody = other.attachedRigidbody;
			joint.anchor = sourceObject.transform.InverseTransformPoint(transform.position - transform.up * overlapOffset * transform.lossyScale.x);

			joint.breakForce = breakForce;
			joint.breakTorque = breakTorque;

			breakListener = sourceObject.gameObject.AddComponent<JointBreakListener>();
			breakListener.JointBreak += Break;
			return true;
		}
		else
		{
			//Break();
		}
		return false;
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
					transform.position = hit.point;
					AttachToObject(body);

					transform.position -= velocity.normalized * hitMove * numberOfHits;
					StickNailToSomething();

					isFlying = false;
				}
			}
			else if (flyingTimer > 10f)
			{
				Destroy(this.gameObject);
			}

			//var colliders = Physics.OverlapSphere(transform.position - transform.up * startOffset * transform.lossyScale.x, overlapRadius * transform.lossyScale.x);
			//var body = colliders.Select(c => c.attachedRigidbody).FirstOrDefault(b => b != null);
			//if (body != null)
			//{
			//	AttachToObject(body);
			//	isFlying = false;
			//}
			//else if (flyingTimer > 10f)
			//{
			//	Destroy(this.gameObject);
			//}

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
