using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {
  public Transform BulletLaunchPoint;
  public Signal BulletSpawnSignal;
  public float ShootInterval = 0.5f;
  public int health;

  PlayerInput PlayerInput;
  float LastBulletLaunchTime;

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

  void Start() {
    PlayerInput = GetComponent<PlayerInput>();
  }

  // Update is called once per frame
  void Update() {
    Shoot();
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
