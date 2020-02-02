using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class timerGameOver : MonoBehaviour
{
    public GameObject controller;  // object to which levelController script is attached to
    float timer;
    public Text txt; // text object for timer
    bool isDead = true;
    bool done = false;
    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        if (isDead && !done)
        {
            timer = controller.GetComponent<LevelController>().Timer;
            txt.text += timer.ToString();
            done = true;
        }
    }
}
