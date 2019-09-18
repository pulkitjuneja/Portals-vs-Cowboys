using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletTypes {
  Bullet = 0,
  Shuriken = 1
}

public class BulletManager : MonoBehaviour {

  public GameObject[] BulletPrefabs;
  public Signal BulletSpawnSignal, BulletDestroySignal;
  Dictionary<int, int> PlayerBulletCount;
  public int MaxBulletsPerPerson = 100;

  void Start() {
    PlayerBulletCount = new Dictionary<int, int>();
    BulletSpawnSignal.addListener(spawnBullet);
    BulletDestroySignal.addListener(removeBulletFromPool);
  }

  void spawnBullet(SignalData data) {
    Transform launchPoint = data.get<Transform>("launchPoint");
    int playerHashCode = data.get<int>("playerHashCode");
    int arenaId = data.get<int>("arenaId");
    int bulletType = (int)data.get<BulletTypes>("bulletType");
    if (!PlayerBulletCount.ContainsKey(playerHashCode)) {
      PlayerBulletCount.Add(playerHashCode, 0);
    }
    if (PlayerBulletCount[playerHashCode] < MaxBulletsPerPerson) {
      GameObject bullet = Instantiate(BulletPrefabs[bulletType], launchPoint.position, Quaternion.identity);
      Bullet bulletScript = bullet.GetComponent<Bullet>();
      bulletScript.initialize(launchPoint.position, launchPoint.forward, arenaId,
       playerHashCode);
      PlayerBulletCount[playerHashCode]++;
    }
  }

  void removeBulletFromPool(SignalData data) {
    Bullet bullet = data.get<Bullet>("Bullet");
    PlayerBulletCount[bullet.PlayerHashCode]--;
  }

}
