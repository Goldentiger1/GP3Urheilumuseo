using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_Button : UI_Element
{
    public UnityEvent ClickEvent;

    private Vector2 defaultSize;
    private Vector2 hoverSize;

    private void Awake()
    {
        defaultSize = transform.localScale;
        hoverSize = new Vector2(defaultSize.x + 0.1f, defaultSize.y + 0.1f);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (ClickEvent != null)
        {
            ClickEvent.Invoke();
            AudioPlayer.Instance.PlayClipAtPoint(1, "UIButtonConfirm", transform.localPosition);
        } 
        else
        {
            AudioPlayer.Instance.PlayClipAtPoint(1, "UIButtonDenied", transform.localPosition);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        print(gameObject.name);
        transform.localScale = hoverSize;
        AudioPlayer.Instance.PlayClipAtPoint(1, "UIButtonHover", transform.localPosition);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = defaultSize;
    }
}
