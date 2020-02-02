using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AK.Wwise.Event Music;
    // Start is called before the first frame update
    void Start()
    {
        Music.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
