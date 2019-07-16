using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  // public variables
  public Rigidbody rigidBody;
  public int movementSpeed = 10;
  public bool CanMove = true;

  PlayerInput PlayerInput;

  Plane playerPlane;

  protected void Rotate() {
    if (PlayerInput.isKeyboard) {
      RotateKeyboard();
    } else {
      RotateJoystick();
    }
  }

  void RotateKeyboard() {
    var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    float intersect = 0.0f;
    if (playerPlane.Raycast(mouseRay, out intersect)) {
      Vector3 targetpoint = mouseRay.GetPoint(intersect);
      transform.rotation = Quaternion.LookRotation(targetpoint - transform.position);
    }
  }

  void RotateJoystick() {
    var verticalView = Input.GetAxis(PlayerInput.VerticalViewAxis);
    var horizontalView = Input.GetAxis(PlayerInput.HorizontalViewAxis);
    if (verticalView != 0 || horizontalView != 0) {
      Debug.Log(verticalView);
      Debug.Log(horizontalView);
      transform.eulerAngles = new Vector3(0, Mathf.Atan2(Input.GetAxis(PlayerInput.VerticalViewAxis), Input.GetAxis(PlayerInput.HorizontalViewAxis)) * 180 / Mathf.PI, 0);
    }
  }

  void MoveForward() {
    if (CanMove) {
      Vector3 MoveVector = new Vector3(Input.GetAxis(PlayerInput.HorizontalAxis), 0, Input.GetAxis(PlayerInput.VerticalAxis));
      rigidBody.velocity = MoveVector * movementSpeed;
    }
  }

  void Awake() {
    rigidBody = GetComponent<Rigidbody>();
    playerPlane = new Plane(Vector3.up, transform.position);
    PlayerInput = GetComponent<PlayerInput>();
  }

  void Update() {
    MoveForward();
  }

  void LateUpdate() {
    Rotate();
  }

  void FixedUpdate() {
  }

}
