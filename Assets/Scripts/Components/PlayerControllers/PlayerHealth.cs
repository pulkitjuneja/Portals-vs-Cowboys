using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {


  public int health = 2;
  public int ArenaId;

  public Image HealthIndicator;
  public Animator HealthIndicatorAnimator;

  void ToggleHealthIndicatorUI(int health) {
    if (health == 1) {
      HealthIndicator.color = Color.red;
      HealthIndicatorAnimator.SetBool("IsHurt", true);

      // keeping this to adda future armor powerup later
    } else if (health == 1) {
      HealthIndicator.color = Color.cyan;
      HealthIndicatorAnimator.SetBool("IsHurt", false);
    }
  }

  public void TakeDamage(Bullet bullet) {
    if (bullet.ArenaId == ArenaId) {
      if (health == 1)
        health = 0;
    } else {
      health--;
    }
    health--;
    ToggleHealthIndicatorUI(health);
    if (health == 0) {
      Destroy(this.gameObject);
    }
  }


  void OnCollisionEnter(Collision collision) {
    Collider other = collision.collider;
    if (other.tag == "Projectiles") {
      var bullet = other.gameObject.GetComponent<Bullet>();
      TakeDamage(bullet);
      bullet.DestroyBullet();
    }
  }
}
