using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDodgeBase : MonoBehaviour {

  PlayerInput PlayerInput;
  public float DodgeDelayInterval;
  public float DodgeDuration;

  bool CanDodge = true;
  float DodgeDelayTimer;
  float Dodgetimer;

  protected virtual void Start() {
    PlayerInput = GetComponent<PlayerInput>();
  }

  protected void Dodge() {
    if (Input.GetButtonDown(PlayerInput.Dodge) && CanDodge) {
      PerformDodge();
      CanDodge = false;
      DodgeDelayTimer = DodgeDelayInterval;
      Dodgetimer = DodgeDuration;
    }

    // replenish dodge timer allow player to dodge again
    if (DodgeDelayTimer > 0) {
      DodgeDelayTimer -= Time.deltaTime;
      if (DodgeDelayTimer <= 0) {
        CanDodge = true;
      }
    }

    // stop dodge and restore actual state
    if (Dodgetimer > 0) {
      Dodgetimer -= Time.deltaTime;
      if (Dodgetimer <= 0) {
        OnDodgeComplete();
      }
    }
  }

  protected virtual void Update() {
    Dodge();
  }


  public abstract void PerformDodge();

  public abstract void OnDodgeComplete();

}
