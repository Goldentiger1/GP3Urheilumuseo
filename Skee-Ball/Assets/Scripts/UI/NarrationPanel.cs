using UnityEngine;

public class NarrationPanel : MonoBehaviour
{
    private LocalizedText narrationText;

    private void Awake()
    {
        narrationText = GetComponent<LocalizedText>();


    }
}
