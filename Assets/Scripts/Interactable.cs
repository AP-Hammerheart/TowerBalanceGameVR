using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    //SoundStuff
    public AK.Wwise.Event Impact;

    public Rigidbody rb;
    public AK.Wwise.Event FlyingObject;
    float speed;

    public HandInteractionScriptBase activeHand = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        FlyingObject.Post(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(rb.velocity.magnitude > 5)
        {
            Impact.Post(gameObject);
        }
    }

    private void Update()
    {
        speed = rb.velocity.magnitude;
        AkSoundEngine.SetRTPCValue("Speed", speed, gameObject);

    }

}