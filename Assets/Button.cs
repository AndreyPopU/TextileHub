using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class Button : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public ButtonGroup buttonGroup;

    public Image background;

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonGroup.OnButtonSelected(this); 
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
         buttonGroup.OnButtonEnter(this); 
    }
     
    public void OnPointerExit(PointerEventData eventData)
    {
         buttonGroup.OnButtonExit(this); 
    }
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        background = GetComponent<Image>();
        buttonGroup.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
