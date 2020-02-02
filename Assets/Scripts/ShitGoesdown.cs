using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShitGoesdown : MonoBehaviour
{
    public List<GameObject> ToNukeWith;
    public Transform NukePoint;

    public void Nuke()
    {
        NukeMore();
        NukeMore();
        NukeMore();
    }

    private void NukeMore()
    {
        foreach (GameObject obj in ToNukeWith)
        {
            var t = Instantiate(obj, RandomizeSpawn(NukePoint.position), new Quaternion(0, 0, 0, 0));
            t.gameObject.layer = 8;
            t.GetComponent<Rigidbody>().AddForce(Vector3.down);
            t.GetComponentInChildren<Rigidbody>().AddForce(Vector3.down);
        }
    }

    private Vector3 RandomizeSpawn(Vector3 pos)
    {
        Vector3 newPos = new Vector3(pos.x + Random.Range(-2f, 2f), pos.y + Random.Range(-2f, 2f), pos.z + Random.Range(-2f, 2f));
        return newPos;
    }
}