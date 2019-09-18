using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
  public float Velocity = 20;
  public int Lifetime = 8;
  public Rigidbody Rigidbody;
  public int ArenaId { get; set; }
  public int PlayerHashCode;
  public Signal BulletDestroySignal;

  public void initialize(Vector3 position, Vector3 direction,
   int arenaId, int playerHashCode) {
    setRotationAndVelocity(position, direction);
    ArenaId = arenaId;
    PlayerHashCode = playerHashCode;
    StartCoroutine(KillSwitch());
  }
  public void setRotationAndVelocity(Vector3 position, Vector3 direction) {
    transform.position = position;
    Rigidbody.velocity = direction * Velocity;
  }

  void Awake() {
    Rigidbody = GetComponent<Rigidbody>();
  }

  IEnumerator KillSwitch() {
    yield return new WaitForSeconds(Lifetime);
    DestroyBullet();
  }

  public void DestroyBullet() {
    SignalData data = new SignalData();
    data.set("Bullet", this);
    BulletDestroySignal.fire(data);
    Destroy(this.gameObject);
  }
}
