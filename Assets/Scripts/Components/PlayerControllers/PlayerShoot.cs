using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {
  public Transform BulletLaunchPoint;
  public Signal BulletSpawnSignal;
  public Signal PortalSpawnSignal;
  public float ShootInterval = 0.5f;
  public float PortalShootInterval = 0.5f;
  public int health;
  // TODO: come up with a better approach
  public int ArenaId;
  public Color CurrentPortalColor;

  PlayerInput PlayerInput;
  float LastBulletLaunchTime;
  float LastPortalLaunchTime;

  int shootLayerMask;

  void Start() {
    PlayerInput = GetComponent<PlayerInput>();
    shootLayerMask = (1 << LayerMask.NameToLayer("Wall")) | (1 << LayerMask.NameToLayer("Portal"));
  }

  void Shoot() {
    if (Input.GetButtonDown(PlayerInput.Fire)) {
      float timeSinceLastLaunch = Time.time - LastBulletLaunchTime;
      if (timeSinceLastLaunch > ShootInterval) {
        SignalData spawnData = new SignalData();
        spawnData.set("LaunchPoint", BulletLaunchPoint);
        spawnData.set("PlayerHashCode", this.GetHashCode());
        BulletSpawnSignal.fire(spawnData);
        LastBulletLaunchTime = Time.time;
      }
    }
  }

  void ShootPortal() {
    if (Input.GetButtonDown(PlayerInput.FirePortal)) {
      float timeSinceLastLaunch = Time.time - LastPortalLaunchTime;
      if (timeSinceLastLaunch > PortalShootInterval) {
        RaycastHit raycastHit;
        bool raycastResult = Physics.Raycast(BulletLaunchPoint.position, BulletLaunchPoint.forward,
        out raycastHit, Mathf.Infinity, shootLayerMask);
        if (raycastResult) {
          if (raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Portal")) {
            return;
          }
          SignalData portalSpawnData = new SignalData();
          portalSpawnData.set("SpawnPosition", raycastHit.point);
          portalSpawnData.set("SpawnDirection", raycastHit.normal);
          portalSpawnData.set("ArenaId", ArenaId);
          portalSpawnData.set("PortalColor", new Color(1.2f, 0, 0));
          PortalSpawnSignal.fire(portalSpawnData);
          LastPortalLaunchTime = Time.time;
        }
      }
    }
  }

  // Update is called once per frame
  void Update() {
    Shoot();
    ShootPortal();
  }

  void OnCollisionEnter(Collision collision) {
    Collider other = collision.collider;
    if (other.tag == "Projectiles") {
      var bullet = other.gameObject.GetComponent<Bullet>();
      TakeDamage(bullet);
      bullet.DestroyBullet();
    }
  }

  void TakeDamage(Bullet bullet) {
    if (bullet.ParentPlayerHashCode == this.GetHashCode()) {
      if (health == 1)
        health = 0;
    } else {
      health--;
    }
    if (health == 0) {
      // kill
    }
  }
}
