using UnityEngine;

public class StreetSounds : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Fabric.EventManager.Instance.PostEvent("ballbounce");
        }
    }

    private void Start()
    {
        Fabric.EventManager.Instance.PostEvent("stop");

        Fabric.EventManager.Instance.PostEvent("street");
    }
}