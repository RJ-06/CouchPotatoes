using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;
using System.Collections;

public class FXManager : MonoBehaviour
{
    

    [Tooltip("Drag in the Camera")]
    [SerializeField] CinemachineCamera c;
    [SerializeField] CinemachineBasicMultiChannelPerlin cmcp;
    private float timer;
    [Tooltip("Audio Source - create and attach to this obj")]
    [SerializeField] AudioSource audioSource;
    [InspectorLabel("Note - fx name, particle effect, and audio clip must be paired by index")]
    [Tooltip("List of overall effect names")]
    [SerializeField] string[] fxType;
    [Tooltip("list of particle effects")]
    [SerializeField] ParticleSystem[] particleList;
    [Tooltip("list of paired sound effects")]
    [SerializeField] AudioClip[] soundEffectList;
    private Dictionary<string, ParticleSystem> particleDict;
    private Dictionary<string, AudioClip> soundEffectDict;

    [Tooltip("event list - you can pair particle effects/audio with camera shake here")]
    [SerializeField] UnityEvent[] effects;

    // Start is called once before the first execution of Update after the MonoBehavior is created
    void Start()
    {
        for (int i = 0; i < fxType.Length; i++) 
        {
            particleDict.Add(fxType[i], particleList[i]);
            soundEffectDict.Add(fxType[i], soundEffectList[i]);
        }
        stopShaking();
    }

    public void playParticle(string s) 
    {
        particleDict[s].Play();
        audioSource.clip = soundEffectDict[s];
        audioSource.Play();
        
    }

    public void screenShake(float intensity, float time) 
    {
        cmcp.AmplitudeGain = intensity;
        timer = time;
        StartCoroutine("startShaking");
    }

    void stopShaking()
    {
        cmcp.AmplitudeGain = 0f;
        timer = 0;
    }

    IEnumerator startShaking(float shakeTime) 
    {
        yield return new WaitForSeconds(shakeTime);
        stopShaking();
    }

    
    
}
