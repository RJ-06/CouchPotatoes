using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;
using System.Collections;

public class FXManager : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////

    [Tooltip("Drag in the Camera")]
    //[SerializeField] CinemachineCamera c;
    //[SerializeField] CinemachineBasicMultiChannelPerlin cmcp;
    private float timer;
    [Tooltip("Audio Source - create and attach to this obj")]
    private AudioSource audioSource;
    [Header("Note - fx name, particle effect, and audio clip must be paired by index")]
    [Tooltip("List of overall effect names")]
    [SerializeField] string[] fxType;
    [Tooltip("List of particle effects")]
    [SerializeField] ParticleSystem[] particleList;
    [Tooltip("List of paired sound effects")]
    [SerializeField] AudioClip[] soundEffectList;
    private Dictionary<string, ParticleSystem> particleDict;
    private Dictionary<string, AudioClip> soundEffectDict;

    [Tooltip("Event list - you can pair particle effects/audio with camera shake here")]
    public UnityEvent[] effects;

    // Start is called once before the first execution of Update after the MonoBehavior is created
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        particleDict = new Dictionary<string, ParticleSystem>();
        soundEffectDict = new Dictionary<string, AudioClip>();

        for (int i = 0; i < fxType.Length; i++) 
        {
            // Debug.Log($"{i}: {fxType[i]}");
            // Debug.Log($"{i}: {particleList[i]}");
            particleDict.Add(fxType[i], particleList[i]);
            soundEffectDict.Add(fxType[i], soundEffectList[i]);
        }
        //stopShaking();
    }
    

    public void playParticle(string s) 
    {
        //if (!particleDict.ContainsKey(s)) return;

        Debug.Log(particleDict);
        Debug.Log(soundEffectDict);

        particleDict[s].Play();
        //if (!soundEffectDict.ContainsKey(s)) return;
        audioSource.clip = soundEffectDict[s];
        audioSource.Play();
        
    }

    //public void screenShake(float intensity, float time) 
    //{
    //    cmcp.AmplitudeGain = intensity;
    //    timer = time;
    //    StartCoroutine("startShaking");
    //}

    //void stopShaking()
    //{
    //    cmcp.AmplitudeGain = 0f;
    //    timer = 0;
    //}

    //IEnumerator startShaking(float shakeTime) 
    //{
    //    yield return new WaitForSeconds(shakeTime);
    //    stopShaking();
    //}

    
    
}
