using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFluid : MonoBehaviour {

  public Signal PortalFluidUsedSignal;
  public Animator animator;

  Vector3 Location;
  Color fluidColor;
  int Arenaid;
  float fadeOutAnimationTime;

  public void ResetProperties(Color color, Vector3 location, int Arenaid) {
    // set rendererr and particle colors
    GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    GetComponent<Renderer>().material.color = color;
    animator = GetComponent<Animator>();
    fluidColor = color;
    fadeOutAnimationTime = 0.1f;
    this.Location = location;
    this.Arenaid = Arenaid;
  }

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
      PlayerShoot playerShoot = other.gameObject.GetComponent<PlayerShoot>();
      if (playerShoot != null) {
        playerShoot.CurrentPortalColor = fluidColor;
        SignalData fluidUsedData = new SignalData();
        fluidUsedData.set("location", Location);
        fluidUsedData.set("arenaId", Arenaid);
        PortalFluidUsedSignal.fire(fluidUsedData);
        StartCoroutine(DestroyAfterAnimation());
      }
    }
  }

  IEnumerator DestroyAfterAnimation() {
    animator.SetBool("destroy", true);
    yield return new WaitForSeconds(fadeOutAnimationTime);
    Destroy(this.gameObject);
  }
}
