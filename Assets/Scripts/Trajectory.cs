using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trajectory : MonoBehaviour
{
    //SoundStuff
    public AK.Wwise.Event Fire;

    private IEnumerator coroutine;
    public float v = 1.0f;
    public Transform target;
    public Transform targetRandom;
    public Vector3 targetRandom2;
    public Transform OriginPoint;
    public float t = 10.0f;
    public float waitTime = 4f;
    public int x_randomise = 30;
    public int y_randomise = 30;
    public int z_randomise = 30;
    public GameObject[] prefabs;
    int len;
    public float torque = 400;
    public bool ToolCannon;
    bool onStart = false;
    public float probabilityToMiss = 30;
    public GameObject explosion;
	public Transform noTargetDir;

	// Start is called before the first frame update
	void Start()
    {
        onStart = true;
		if(targetRandom != null)
        targetRandom2 = targetRandom.position;
        StartCoroutine(Countdown(waitTime));
        len = prefabs.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown("a"))
        {
            shoot();
        }

    }

	//private void OnDestroy()
	//{
	//	StopCoroutine(KillMe());
	//}

	//private IEnumerator KillMe()
	//{
	//	yield return new WaitForSeconds(11f);
	//	Destroy(gameObject);
	//}

    private IEnumerator Countdown(float waitTime)
    {
        while (onStart)
        {
			if (noTargetDir != null)
			{
				yield return new WaitForSeconds(waitTime * Random.Range(0.5f, 2f));
			}
			else
			{
				yield return new WaitForSeconds(waitTime);
			}

			try
			{
				shoot();
			}
			catch (System.Exception e)
			{
				Debug.LogException(e);
			}

        }
        
    }

    void shoot()
    {
        //SoundStuff
        Fire.Post(gameObject);

        Instantiate(explosion, OriginPoint.position, Quaternion.identity);
        GameObject thisobject = Instantiate(prefabs[Random.Range(0,len)], noTargetDir != null ? noTargetDir.position : OriginPoint.position, Quaternion.identity) as GameObject;

		//if (thisobject.GetComponent<Trajectory>() != null)
		//{
		//	StartCoroutine(thisobject.GetComponent<Trajectory>().KillMe());
		//}

        if (ToolCannon)
        {
            float variable = Random.Range(0.5f, 4f);
            Vector3 scaleChange = new Vector3(thisobject.transform.localScale.x * variable, thisobject.transform.localScale.y * variable, thisobject.transform.localScale.z * variable);
            thisobject.transform.localScale = scaleChange;
        }

		if (target != null)
		{
			//Rigidbody rb = thisobject.AddComponent<Rigidbody>();
			Rigidbody rb = thisobject.GetComponent<Rigidbody>();
			var CannonToTarget = target.position - OriginPoint.position;
			rb.AddTorque(transform.forward * torque);
			rb.useGravity = true;
			if (Random.Range(0, 101) <= probabilityToMiss)
			{
				targetRandom.position = new Vector3(targetRandom.position.x + Random.Range(-x_randomise, x_randomise), targetRandom.position.y + Random.Range(0, y_randomise), targetRandom.position.z + Random.Range(-z_randomise, z_randomise));
				CannonToTarget = targetRandom.position - OriginPoint.position;
				targetRandom.position = targetRandom2;
				Debug.Log("random");
			}
			var toTarget_xz = CannonToTarget;
			toTarget_xz.y = 0;
			var toTarget_y = Vector3.up * CannonToTarget.y;
			var v_ground = toTarget_xz * (1 / t);
			var v_up = toTarget_y * (1 / t) - Physics.gravity * t / 2;
			rb.velocity = v_ground + v_up;
		}
		else
		{
			//Rigidbody rb = thisobject.AddComponent<Rigidbody>();
			Rigidbody rb = thisobject.GetComponent<Rigidbody>();
			rb.AddTorque(transform.forward * torque);
			rb.useGravity = true;

			var power = Random.Range(7f, 14f);
			rb.velocity = (noTargetDir.transform.forward + Random.insideUnitSphere * 0.3f) * power;

			var myRb = GetComponent<Rigidbody>();
			if (myRb != null)
			{
				myRb.AddForceAtPosition(-noTargetDir.transform.forward * power, noTargetDir.position * 0.3f, ForceMode.Impulse);
			}
		}
    }
}