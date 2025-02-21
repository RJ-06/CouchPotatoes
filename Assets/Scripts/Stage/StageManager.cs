using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Tooltip("Every x seconds in the list, execute the event in eventList")]
    public float[] timingList;
    [Tooltip("events that go off in the stage")]
    public UnityEvent[] eventList;
    [SerializeField] float randomTiming;

    int ind = 0;
    float timer = 0f;

    void Start()
    {
        for (int i = 0; i < timingList.Length; i++) 
        {
            timingList[i] += UnityEngine.Random.Range(-randomTiming,randomTiming);
        }
    }

    // Update is called once per frame
    void Update()
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
