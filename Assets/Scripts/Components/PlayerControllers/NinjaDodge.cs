using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaDodge : PlayerDodgeBase {

  public Animator Animator;
  public GameObject DeflectorShield;
  protected override void Start() {
    base.Start();
  }

  public override void PerformDodge() {
    Animator.SetTrigger("Dodge");
    DeflectorShield.SetActive(true);
  }

  public override void OnDodgeComplete() {
    DeflectorShield.SetActive(false);
  }

  protected override void Update() {
    base.Update();
  }
}
