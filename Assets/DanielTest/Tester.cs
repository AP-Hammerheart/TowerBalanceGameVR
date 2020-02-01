using System;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
	public float mouseStiffness;
	public float mouseDamping;
	public Nail nailPrefab;
	public List<GameObject> props;

	private Rigidbody grabbedObject;
	private float grabbedDistance;
	private Vector3 grabPosition;
	private Vector3 grabbeObjectLocal;
	private SpringJoint grabbedJoint;
	private Vector3 currentRotation;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.Log("Ray " + ray);

			if (Physics.Raycast(ray, out var hit))
			{
				if (hit.rigidbody != null)
				{
					Debug.Log("Hit " + hit.rigidbody.gameObject.name);
					Grab(hit.rigidbody, hit);
				}
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			Debug.Log("Release ");
			Release();
		}

		if (Input.GetMouseButton(1))
		{
			currentRotation.x += Input.GetAxis("Mouse X") * 3;
			currentRotation.y -= Input.GetAxis("Mouse Y") * 3;
			currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
			currentRotation.y = Mathf.Clamp(currentRotation.y, -90, 90);
			Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);

			if (Input.GetKey(KeyCode.W))
			{
				Camera.main.transform.position += Camera.main.transform.forward * 5f * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.S))
			{
				Camera.main.transform.position -= Camera.main.transform.forward * 5f * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.A))
			{
				Camera.main.transform.position -= Camera.main.transform.right * 5f * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.D))
			{
				Camera.main.transform.position += Camera.main.transform.right * 5f * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.E))
			{
				Camera.main.transform.position += Camera.main.transform.up * 5f * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.Q))
			{
				Camera.main.transform.position -= Camera.main.transform.up * 5f * Time.deltaTime;
			}
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.Log("Ray " + ray);

			if (Physics.Raycast(ray, out var hit))
			{
				if (hit.rigidbody != null)
				{
					var nail = Instantiate(nailPrefab, hit.point,
						Quaternion.LookRotation(Vector3.Cross(hit.normal, UnityEngine.Random.onUnitSphere), hit.normal));

					nail.AttachToObject(hit.rigidbody);
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.Log("Ray " + ray);

			if (Physics.Raycast(ray, out var hit))
			{
				if (hit.rigidbody != null)
				{
					var nail = Instantiate(nailPrefab, hit.point,
						Quaternion.LookRotation(Vector3.Cross(-ray.direction, UnityEngine.Random.onUnitSphere), -ray.direction));

					nail.AttachToObject(hit.rigidbody);
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Instantiate(props[UnityEngine.Random.Range(0, props.Count)], transform.position, UnityEngine.Random.rotation);
		}

		grabbedDistance += Input.mouseScrollDelta.y * 0.2f;
	}

	private void FixedUpdate()
	{
		if (grabbedObject != null)
		{
			var grabPos = grabbedObject.transform.TransformPoint(grabbeObjectLocal);
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var targetPos = ray.origin + ray.direction * grabbedDistance;

			var toTarget = (targetPos - grabPos);
			grabbedObject.AddForceAtPosition(mouseStiffness * toTarget - grabbedObject.velocity * mouseDamping, grabPos, ForceMode.Acceleration);
			grabbedObject.AddTorque(-grabbedObject.angularVelocity * 0.5f, ForceMode.Acceleration);
		}
	}

	private void Grab(Rigidbody rigidbody, RaycastHit hit)
	{
		grabbedDistance = hit.distance;
		grabPosition = hit.point;
		grabbedObject = rigidbody;
		grabbeObjectLocal = rigidbody.transform.InverseTransformPoint(hit.point);
	}

	private void Release()
	{
		grabbedObject = null;
	}
}
