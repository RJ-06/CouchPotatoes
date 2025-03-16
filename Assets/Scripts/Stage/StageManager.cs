using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Tooltip("Every x seconds in the list, execute the event in eventList")]
    public float[] timingList;
    [Tooltip("Events that go off in the stage")]
    public UnityEvent[] eventList;
    [SerializeField] float randomTiming;

    private int ind = 0;
    private float timer = 0f;

    void Start()
    {
        for (int i = 0; i < timingList.Length; i++) 
        {
            timingList[i] += UnityEngine.Random.Range(-randomTiming,randomTiming);
        }
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= timingList[ind] && ind < eventList.Length) 
        {
            timer = 0;
            eventList[ind].Invoke();
            ind++;
        }
    }
}
