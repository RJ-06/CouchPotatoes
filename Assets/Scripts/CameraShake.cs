using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    // allows to be called from anywhere
    public static CameraShake Instance;

    public CinemachineImpulseSource impulseSource;

    // private CinemachineVirtualCamera virtualCam;
    // private CinemachineBasicMultiChannelPerlin noise;
    // private float shakeDur;

    void Awake()
    {
        Instance = this;
        // virtualCam = GetComponent<CinemachineVirtualCamera>();
        // if (virtualCam == null)
        // {
        //     Debug.LogError("CameraShake Script: No CinemachineVirtualCamera Found");
        //     return;
        // }
        // noise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        // if (noise == null){
        //     Debug.LogError("CameraShake Script: No CinemachineVirtualCamera Found");
        // }
    }
    
    public void Shaking(float strength)
    {
        // Debug.Log("shaking");
        impulseSource.GenerateImpulse(strength);
    }

    // public void CameraShaking(float strength, float time)
    // {
    //     if(noise == null)
    //     {
    //         return;
    //     }
    //     noise.AmplitudeGain = strength;
    //     shakeDur = time;
    // }

    // void Update()
    // {
    //     if (shakeDur > 0)
    //     {
    //         shakeDur -= Time.deltaTime;
    //         if (shakeDur <= 0f && noise != null)
    //         {
    //             noise.AmplitudeGain = 0f;
    //         }
    //     }
    // }
}
