using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    public List<Button> Buttons;
    public Sprite buttonIdle;
    public Sprite buttonHover;
    public Sprite buttonActive;

  public void Subscribe(Button button) 
  {
        if(Buttons == null)
        {
            Buttons = new List<Button>();
        } 

        Buttons.Add(button);
  }

  public void OnButtonEnter(Button button)
  {
        ResetButtons();
        button.background.sprite = buttonHover;
  }

  public void OnButtonExit(Button button)
  {
        ResetButtons();
  }

  public void OnButtonSelected(Button button)
  {
        ResetButtons();
        button.background.sprite = buttonActive;
  }

  public void ResetButtons()
  {
        foreach(Button button in Buttons)
        {
            button.background.sprite = buttonIdle;
        }
  }

}
