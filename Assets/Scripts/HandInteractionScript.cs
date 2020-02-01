using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class HandInteractionScript : MonoBehaviour
{
    //public BoxCollider collider;
    //public SteamVR_TrackedObject trackObj;
    //private GameObject heldItem;
    //private GameObject touchedItem;

    public SteamVR_Action_Boolean grabAction;
    private SteamVR_Behaviour_Pose pose;
    private FixedJoint joint;
    private Interactable currentInteractable;
    public List<Interactable> contactInteractables = new List<Interactable>();

    private void Awake()
    {
        //trackObj = GetComponent<SteamVR_TrackedObject>();
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        joint = GetComponent<FixedJoint>();
        //Debug.Log(pose);
        //Debug.Log(joint);
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (grabAction.GetStateDown(pose.inputSource))
        {
            Grab();
        }        
        if (grabAction.GetStateUp(pose.inputSource))
        {
            Release();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            contactInteractables.Add(other.gameObject.GetComponent<Interactable>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            contactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
        }
    }

    public void Grab()
    {
        currentInteractable = GetNearestInteractable();

        if (!currentInteractable)
            return;

        if (currentInteractable.activeHand)
            currentInteractable.activeHand.Release();

        currentInteractable.transform.position = transform.position;

        Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
        joint.connectedBody = targetBody;

        currentInteractable.activeHand = this;
    }

    public void Release()
    {
        if (!currentInteractable)
            return;

        Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = pose.GetVelocity();
        targetBody.angularVelocity = pose.GetAngularVelocity();

        joint.connectedBody = null;
        currentInteractable.activeHand = null;
        currentInteractable = null;

    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach (Interactable inc in contactInteractables)
        {
            distance = (inc.transform.position - transform.position).sqrMagnitude;

            if(distance < minDistance)
            {
                minDistance = distance;
                nearest = inc;
            }
        }

        return nearest;
    }
}