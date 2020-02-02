using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public GameObject SmallExplosion;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(explode());
        }
    }


    IEnumerator explode()
    {
        yield return new WaitForSeconds(8);
        Instantiate(SmallExplosion, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}