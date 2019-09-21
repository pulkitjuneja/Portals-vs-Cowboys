using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RotationControlTypes {
  BillBoard,
  Fixed,
}

public class RotationController : MonoBehaviour {

  public float OffsetToCamera;
  public RotationControlTypes RotationControlType;

  Quaternion IntialRotation;
  Camera LookatCamera;
  Vector3 LocalStartPosition;

  void FixRotation() {
    this.transform.rotation = IntialRotation;
  }

  void BillBoard() {
    transform.LookAt(transform.position + LookatCamera.transform.rotation * Vector3.forward, LookatCamera.transform.rotation * Vector3.up);
    transform.localPosition = LocalStartPosition;
    transform.position = transform.position + transform.rotation * Vector3.forward * OffsetToCamera;
  }

  void Awake() {
    LookatCamera = Camera.main;
    IntialRotation = transform.rotation;
    LocalStartPosition = transform.localPosition;
  }


  void LateUpdate() {
    if (RotationControlType == RotationControlTypes.Fixed) {
      FixRotation();
    } else if (RotationControlType == RotationControlTypes.BillBoard) {
      BillBoard();
    }
  }

}
