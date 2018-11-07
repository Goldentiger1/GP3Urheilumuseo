using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singelton<UIManager>
{
    public Transform HUDCanvas { get; private set; }

    private Transform panel;

    private void Awake()
    {
        HUDCanvas = GameObject.Find("HUDCanvas").transform;

        panel = HUDCanvas.transform.Find("Panel");
    }
}
