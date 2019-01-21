using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singelton<AudioManager>
{
    private void Start()
    {
        Fabric.EventManager.Instance.PostEvent("ambient");
    }
}
