using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public AK.Wwise.Event Alarm;

    private Color originalColour;
    public Color flashColour = Color.red;
    private MeshRenderer rend;
    private LevelController lControl;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        if (rend == null)
        {
            rend = GetComponentInChildren<MeshRenderer>();

            if(rend == null)
            {
                rend = GetComponentInChildren<MeshRenderer>().gameObject.GetComponentInChildren<MeshRenderer>();
            }
        }
        originalColour = rend.material.color;
        lControl = FindObjectOfType<LevelController>();
        lControl.breakableObjects.Add(this);
    }

    private void OnDestroy()
    {
        //Sound
        lControl.breakableObjects.Remove(this);
    }

    public void Break()
    {

        StartCoroutine(StartBreaking());
    }

    private void Flash(float t)
    {
        Alarm.Post(gameObject);

		rend.material.color = flashColour;
        Invoke("ResetFlash", t);
    }

    private void ResetFlash()
    {
        rend.material.color = originalColour;
    }

    IEnumerator StartBreaking()
    {
		Flash(0.3f);
        yield return new WaitForSeconds(2f);
        Flash(0.3f);
        yield return new WaitForSeconds(2f);
        Flash(0.3f);
        yield return new WaitForSeconds(1f);
        Flash(0.3f);
        yield return new WaitForSeconds(1f);
        Flash(0.3f);
        yield return new WaitForSeconds(1f);
        Flash(0.3f);
        yield return new WaitForSeconds(1f);
        Flash(0.3f);
        yield return new WaitForSeconds(0.5f);
        Flash(0.3f);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject, 0.5f);
    }
}