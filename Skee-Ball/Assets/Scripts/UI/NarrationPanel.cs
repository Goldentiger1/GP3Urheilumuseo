using UnityEngine;

public class NarrationPanel : Singelton<NarrationPanel>
{
    private LocalizedText narrationText;

    private void Awake()
    {
        narrationText = GetComponent<LocalizedText>();
        narrationText.enabled = false;
    }

    private void OnEnable()
    {
        
    }

    public void ShowPanel(string key)
    {
        narrationText.Key = key;
        narrationText.enabled = true;
    }
}
