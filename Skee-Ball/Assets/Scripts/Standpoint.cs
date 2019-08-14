using UnityEngine;

public class Standpoint : MonoBehaviour
{
    private GameObject SwitchScene_Icon;
    private GameObject Arrow_Icon;
    private GameObject Locked_Icon;
    private GameObject Feet_Icon;

    private void Awake()
    {
        var icons = transform.GetChild(1);
        SwitchScene_Icon = icons.GetChild(0).gameObject;
        Arrow_Icon = icons.GetChild(1).gameObject;
        Locked_Icon = icons.GetChild(2).gameObject;
        Feet_Icon = icons.GetChild(3).gameObject;
    }

    private void Start()
    {
        SwitchScene_Icon.SetActive(false);
        Arrow_Icon.SetActive(false);
        Locked_Icon.SetActive(false);
        Feet_Icon.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(12))
        {
            Debug.LogError("OnTriggerEnter: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(12))
        {
            Debug.LogError("OnTriggerExit: " + other.name);
        }
    }
}
