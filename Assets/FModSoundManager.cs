using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FModSoundManager : Singleton<FModSoundManager>
{
    bool loaded = false;
    string currentEvent;
    FMOD.Studio.EventInstance[] ambiences = new FMOD.Studio.EventInstance[2];
    int currentId = 0;
    float volumnValue = 0.2f;
    public bool isMerged = false;
    public bool getHelpDialogue = false;
    float defaultVolumn = 0.2f;
    bool pressedStart = false;
    //[FMODUnity.EventRef]
    //public string eventName;
    // Start is called before the first frame update

    string[] paramNames = new string[]{
        "To Exploration","To Danger Zone","To Battle"
        };

    void Start()
    {
        //ambience = FMODUnity.RuntimeManager.CreateInstance(eventName);
        //ambience.start();
        // Invoke("delayTest", 0.1f);
        startEvent("event:/HRS 2 Test");
        GetIntoSafeZone();
        DontDestroyOnLoad(gameObject);
    }
    FMOD.Studio.EventInstance currentAmbience()
    {
        return ambiences[currentId];
    }
    public void startEvent(string eventName)
    {

        if (eventName == "")
        {

            currentEvent = eventName;
            return;
        }
        if (eventName != currentEvent)
        {
            Debug.Log("start even " + eventName);
            // if (currentAmbience() == null)
            {

                currentAmbience().stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            currentId++;
            if (currentId > 1)
            {
                currentId = 0;
            }
            ambiences[currentId].release();
            ambiences[currentId] = FMODUnity.RuntimeManager.CreateInstance(eventName);

            // ambience.setVolume(0.1f);
            currentAmbience().start();

            currentAmbience().setVolume(defaultVolumn);
            currentEvent = eventName;
        }
    }
    public void setToZeroExpect(int exp)
    {
        for(int i = 0;i< paramNames.Length; i++)
        {
            if(i == exp)
            {
                SetParam(paramNames[i], 1);
            }
            else
            {

                SetParam(paramNames[i], 0);
            }
        }
    }
    public void GetIntoBattle()
    {
        setToZeroExpect(2);
    }

    public void GetIntoSafeZone()
    {
        setToZeroExpect(0);
    }

    public void GetIntoDangerZone()
    {
        setToZeroExpect(1);
    }
    public void SetParam(string paramName, float value)
    {

        currentAmbience().setParameterByName(paramName, value);
    }

    // Update is called once per frame
    void Update()
    {
        //if (FMODUnity.RuntimeManager.HasBankLoaded("Master") && !loaded)
        //{
        //    loaded = true;
        //    Debug.Log("Master Bank Loaded");
        //    //startEvent("event:/Town - Forest");
        //    //SceneManager.LoadScene(1);

        //}
        //if (pressedStart && loaded)
        //{
        //    pressedStart = false;

        //    SceneManager.LoadScene(1);
        //}
    }
    private void OnDestroy()
    {
        currentAmbience().stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //Destroy(ambience);
    }
    public void startGame()
    {
        pressedStart = true;
    }
}
