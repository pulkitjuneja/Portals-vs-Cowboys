using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFluid : MonoBehaviour {

  public Signal PortalFluidUsedSignal;
  public Animator animator;
  public Renderer SphereRenderer;

  Vector3 Location;
  Color fluidColor;
  int Arenaid;
  float fadeOutAnimationTime;

  public void ResetProperties(Color color, Vector3 location, int Arenaid) {
    // set rendererr and particle colors
    SphereRenderer.material.EnableKeyword("_EMISSION");
    SphereRenderer.material.SetColor("_EmissionColor", color);
    SphereRenderer.material.SetColor("_BaseColor", color);
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
        playerShoot.ChangeCurrentPortalColor(fluidColor);
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
