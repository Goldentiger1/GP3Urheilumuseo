using UnityEngine;

public class NarrationPanel : Singelton<NarrationPanel>
{
    private LocalizedText narrationText;

    private void Awake()
    {
        narrationText = GetComponent<LocalizedText>();
        narrationText.enabled = false;
    }

    public void ShowPanel(string key)
    {
        //Debug.LogError(key);
        narrationText.Key = key;
        narrationText.enabled = true;
    }
}
