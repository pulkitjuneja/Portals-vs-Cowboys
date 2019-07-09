using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

  public Signal PortalTimeoutSignal;
  public MeshRenderer PortalMesh;
  ParticleSystem Vortex;
  public GameObject EmitterParent;
  [HideInInspector]
  public GameObject CorrespondingPortal;
  public Animator PortalAnimator;
  int ArenaId;
  Color PortalColor;
  public int TimeoutInterval = 5;

  Light PortaLight;

  public void initialize(Color color, GameObject correspondingPortal, int ArenaId) {
    PortalMesh.material.SetColor("_portalColor", color);
    ParticleSystem.MainModule main = Vortex.main;
    main.startColor = color;
    this.CorrespondingPortal = correspondingPortal;
    PortaLight.color = color;
    PortalColor = color;
  }

  void Start() {
    StartCoroutine(killSwitch());
  }

  void Awake() {
    Vortex = EmitterParent.GetComponentInChildren<ParticleSystem>();
    PortaLight = GetComponentInChildren<Light>();
  }

  IEnumerator killSwitch() {
    yield return new WaitForSeconds(TimeoutInterval);
    Destroy(this.gameObject);
    SignalData data = new SignalData();
    PortalTimeoutSignal.fire(data);
  }

  public void changeCorrespondingPortal(GameObject otherPortal) {
    CorrespondingPortal = otherPortal;
    PortalAnimator.SetBool("isOpen", CorrespondingPortal ?? false);
  }

  void Update() {
    EmitterParent.transform.Rotate(0, 0, 25);
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
}
