using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFluid : MonoBehaviour {

  public Signal PortalFluidUsedSignal;
  public Animator animator;
  public Renderer SphereRenderer;

  Vector3 Location;
  TwoColorGradient PortalColors;
  int Arenaid;
  float fadeOutAnimationTime;

  public void ResetProperties(TwoColorGradient portalColors, Vector3 location, int Arenaid) {
    // set rendererr and particle colors
    SphereRenderer.material.EnableKeyword("_EMISSION");
    SphereRenderer.material.SetColor("_EmissionColor", portalColors.PrimaryColor);
    SphereRenderer.material.SetColor("_BaseColor", portalColors.PrimaryColor);
    animator = GetComponent<Animator>();
    PortalColors = portalColors;
    fadeOutAnimationTime = 0.1f;
    this.Location = location;
    this.Arenaid = Arenaid;
  }

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
      PlayerShoot playerShoot = other.gameObject.GetComponent<PlayerShoot>();
      if (playerShoot != null) {
        playerShoot.ChangeCurrentPortalColor(PortalColors);
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
