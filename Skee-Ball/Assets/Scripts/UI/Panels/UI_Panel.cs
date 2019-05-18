using UnityEngine;

public abstract class UI_Panel : UI_Element
{
    public virtual void Open()
    {
        AudioPlayer.Instance.PlayClipAtPoint(1, "UIPanelOpen", transform.position);
    }

    public virtual void Close()
    {
        AudioPlayer.Instance.PlayClipAtPoint(1, "UIPanelClose", transform.position);
    }
}
