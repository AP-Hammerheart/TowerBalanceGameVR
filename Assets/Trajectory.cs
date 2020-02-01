using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trajectory : MonoBehaviour
{
    private IEnumerator coroutine;
    public float v = 1.0f;
    public Transform target;
    public Transform targetRandom;
    public Vector3 targetRandom2;
    public Transform OriginPoint;
    public Rigidbody rb;
    public GameObject prefab;
    public float t = 10.0f;
    public float waitTime = 4f;
    public int x_randomise = 30;
    public int y_randomise = 30;
    public int z_randomise = 30;
    public GameObject[] prefabs;
    int len;
    public float torque = 400;

    bool onStart = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        onStart = true;
        targetRandom2 = targetRandom.position;
        StartCoroutine(Countdown(waitTime));
        prefabs = Resources.LoadAll<GameObject>("Prefabs1");
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

    private IEnumerator Countdown(float waitTime)
    {
        while (onStart)
        {
            yield return new WaitForSeconds(waitTime);
            shoot();
        }
        
    }

    void shoot()
    {

        GameObject thisobject = Instantiate(prefabs[Random.Range(0,len)], OriginPoint.position, Quaternion.identity) as GameObject;
        Rigidbody rb = thisobject.AddComponent<Rigidbody>();

        var CannonToTarget = target.position - OriginPoint.position;
        rb.AddTorque(transform.forward * torque);
        rb.useGravity = true;
        if (Random.Range(0, 2) == 1)
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
}