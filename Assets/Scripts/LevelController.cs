using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    //SoundStuff
    public AK.Wwise.Event GOEv;
    public GameObject GameOvertxt;

    public float Timer = 0.0f;
    public float breakTimer = 10f;
    private bool playing = false;
    public float gameOverTime = 5f;
    private int objectsDestroyed = 0;
    private AudioSource audioSource;
    public bool die;
    public ShitGoesdown shit;


    public List<BreakableObject> breakableObjects = new List<BreakableObject>();
    void Start()
    {
        playing = true;
        var t = FindObjectsOfType<BreakableObject>();
        breakableObjects = t.OfType<BreakableObject>().ToList();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(BreakStuff());
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            Timer += Time.deltaTime;
        }
        if (die)
        {
            die = false;
            StartCoroutine(GameOver());
        }
    }

    void UpdateBreakTimer()
    {
        if (objectsDestroyed > 5 && objectsDestroyed <= 10)
        {
            breakTimer = 9f;
        }
        if (objectsDestroyed > 10 && objectsDestroyed <= 15)
        {
            breakTimer = 7f;
        }
        if (objectsDestroyed > 15 && objectsDestroyed <= 20)
        {
            breakTimer = 3f;
        }
        if (objectsDestroyed > 20 && objectsDestroyed <= 25)
        {
            breakTimer = 1f;
        }
    }

    private IEnumerator BreakStuff()
    {
        while (playing)
        {
            yield return new WaitForSeconds(breakTimer);
            breakableObjects.ElementAt(Random.Range(0, breakableObjects.Count)).Break();
            objectsDestroyed++;
            UpdateBreakTimer();
        }
    }

    public IEnumerator GameOver()
    {
        playing = false;
        shit.Nuke();
        shit.Nuke();
        shit.Nuke();
        yield return new WaitForSeconds(2f);
        GOEv.Post(gameObject);
        yield return new WaitForSeconds(gameOverTime - 2f);
        //Todo
        //Game over screeeen
        GameOvertxt.SetActive(true);
        yield return new WaitForSeconds(20f);
        SceneManager.LoadScene("TowerScene");
    }
}