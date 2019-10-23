using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStateMachine {
  IEnumerator CurrentState;
  bool Transitioning;
  MonoBehaviour Component;

  public SimpleStateMachine(IEnumerator initialState, MonoBehaviour component) {
    CurrentState = initialState;
    this.Component = component;
  }

  public IEnumerator Start() {
    Debug.Log(CurrentState);
    while (CurrentState != null) {
      Transitioning = false;
      yield return Component.StartCoroutine(CurrentState);
    }
  }

  public void SetState(IEnumerator newState) {
    this.CurrentState = newState;
    Transitioning = true;
  }
}