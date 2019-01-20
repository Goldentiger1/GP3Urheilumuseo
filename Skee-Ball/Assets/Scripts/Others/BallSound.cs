using UnityEngine;

public class BallSound : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Fabric.EventManager.Instance.PostEvent("ballbounce");
        }
    }

    void Start ()
    {
        Fabric.EventManager.Instance.PostEvent("ambient");
	}
}
