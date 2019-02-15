using UnityEngine;

public class UIManager : Singelton<UIManager>
{
    private Transform HUDCanvas;

    private void Awake() 
    {
        HUDCanvas = transform.Find("HUDCanvas");
    }

    private void Start() 
    {
        HUDCanvas.gameObject.SetActive(false);
    }

    public void ShowText(float duration) 
    {
        HUDCanvas.gameObject.SetActive(true);


        Invoke("HideCanvas", duration);
    }

    public void ShowLoadingImage(float duration)
    {

    }

    private void HideCanvas() 
    {
        HUDCanvas.gameObject.SetActive(false);
    }

    private void Update() 
    {
      
    }
}
