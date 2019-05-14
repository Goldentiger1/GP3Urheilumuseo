using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UI_Element : MonoBehaviour, 
    IPointerEnterHandler, 
    IPointerExitHandler, 
    IPointerDownHandler, 
    IPointerUpHandler, 
    IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
       
    }
}
