using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

  [HideInInspector]
  public string HorizontalAxis;
  [HideInInspector]
  public string VerticalAxis;
  [HideInInspector]
  public string HorizontalViewAxis;
  [HideInInspector]
  public string VerticalViewAxis;
  [HideInInspector]
  public string Fire;
  [HideInInspector]
  public string FirePortal;
  [HideInInspector]
  public string Dodge;
  [HideInInspector]
  public bool isKeyboard;


  public void AssignButtons(int controllerId) {
    HorizontalAxis = "P" + controllerId + "Horizontal";
    VerticalAxis = "P" + controllerId + "Vertical";
    HorizontalViewAxis = "P" + controllerId + "HorizontalView";
    VerticalViewAxis = "P" + controllerId + "VerticalView";
    Fire = "P" + controllerId + "Fire";
    FirePortal = "P" + controllerId + "FirePortal";
    Dodge = "P" + controllerId + "Dodge";
    isKeyboard = controllerId == 1 ? true : false;
  }
}
