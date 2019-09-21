using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.UI;

public class Portal : MonoBehaviour {

  public Signal PortalTimeoutSignal;
  public VisualEffect PortalVFX;
  public VisualEffect IngestionVFX;
  public GameObject EmitterParent;
  [HideInInspector]
  GameObject CorrespondingPortal;
  int ArenaId;
  PortalColorData PortalColors;
  public int TimeoutInterval = 5;

  public Image TimeOutUI;

  Light PortaLight;

  public void initialize(PortalColorData portalColors, GameObject correspondingPortal, int ArenaId) {
    IngestionVFX.SetGradient("TrailColor", portalColors.TrailGradient);
    PortalVFX.SetGradient("PortalColor", portalColors.PrimaryGradient);
    changeCorrespondingPortal(correspondingPortal);
    PortaLight.color = portalColors.PrimaryColor;
    PortalColors = portalColors;
    this.ArenaId = ArenaId;
    InitialzeTimoutUI();
  }

  void Start() {
    StartCoroutine(killSwitch());
  }

  void Awake() {
    PortaLight = GetComponentInChildren<Light>();
  }

  void Update() {
    TimeOutUI.fillAmount -= (Time.fixedDeltaTime / TimeoutInterval);
  }

  IEnumerator killSwitch() {
    yield return new WaitForSeconds(TimeoutInterval);
    Destroy(this.gameObject);
    SignalData data = new SignalData();
    data.set("ArenaId", ArenaId);
    data.set("PortalColors", PortalColors);
    PortalTimeoutSignal.fire(data);
  }

  public void changeCorrespondingPortal(GameObject otherPortal) {
    CorrespondingPortal = otherPortal;
    float ingestionRadius = CorrespondingPortal == null ? 1.3f : 4.21f;
    IngestionVFX.SetFloat("TrailRadius", ingestionRadius);
  }

  void OnTriggerEnter(Collider other) {
    if (other.tag == "Projectiles") {
      var bullet = other.gameObject.GetComponent<Bullet>();
      Vector3 incomingVelocity = bullet.Rigidbody.velocity;
      if (Vector3.Dot(incomingVelocity, transform.forward) < 0) {
        Vector3 launchPoint = CorrespondingPortal.transform.position + CorrespondingPortal.transform.forward * 1.5f;
        Vector3 newDirection = CalculateTeleportDirection(bullet.Rigidbody.velocity);
        bullet.setRotationAndVelocity(launchPoint, newDirection);
      }
    }
  }

  Vector3 CalculateTeleportDirection(Vector3 incomingVelocity) {
    Vector3 reflection;
    float angle = Mathf.Acos(Mathf.Clamp(Vector3.Dot(CorrespondingPortal.transform.forward, transform.forward), -1, 1));
    if (angle > 1.6) {
      reflection = -incomingVelocity;
    } else {
      reflection = Vector3.Reflect(incomingVelocity, transform.forward);
    }
    Quaternion rotationAmount = Quaternion.FromToRotation(transform.forward, CorrespondingPortal.transform.forward);
    Vector3 eulerAngles = rotationAmount.ToEulerAngles();
    // rotationAmount = rotationAmount * Mathf.Rad2Deg;
    //  Debug.Log(rotationAmount);
    Vector3 rotatedVector = rotationAmount * reflection;
    rotatedVector.Normalize();
    return rotatedVector;
  }

  void InitialzeTimoutUI() {
    TimeOutUI.color = new Color(PortalColors.PrimaryColor.r, PortalColors.PrimaryColor.g, PortalColors.PrimaryColor.b, 0.6f);
    Transform uiTransform = TimeOutUI.canvas.transform;
    uiTransform.LookAt(uiTransform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
  }
}
