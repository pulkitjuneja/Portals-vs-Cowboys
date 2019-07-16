using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDodge : PlayerDodgeBase {
  public Rigidbody rigidBody;
  public Animator Animator;

  PlayerMovement PlayerMovement;

  protected override void Start() {
    base.Start();
    PlayerMovement = GetComponent<PlayerMovement>();
    rigidBody = GetComponent<Rigidbody>();
    Animator = GetComponent<Animator>();
  }


  public override void PerformDodge() {
    PlayerMovement.CanMove = false;
    rigidBody.velocity = transform.forward * 30;
    Animator.SetTrigger("Dodge");
  }

  public override void OnDodgeComplete() {
    PlayerMovement.CanMove = true;
  }

  protected override void Update() {
    base.Update();
  }
}
