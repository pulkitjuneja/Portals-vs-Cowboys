using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

  public GameObject BulletPrefab;
  public Signal BulletSpawnSignal, BulletDestroySignal;
  // List<GameObject> Bullets; // replace this with a dynamic pool
  Dictionary<int, int> PlayerBulletCount;
  public int MaxBulletsPerPerson = 100;

  void Start()
  {
    PlayerBulletCount = new Dictionary<int, int>();
    BulletSpawnSignal.addListener(spawnBullet);
    BulletDestroySignal.addListener(removeBulletFromPool);
  }

  void spawnBullet(SignalData data)
  {
    Transform launchPoint = data.get<Transform>("LaunchPoint");
    int playerHashCode = data.get<int>("PlayerHashCode");
    if (!PlayerBulletCount.ContainsKey(playerHashCode))
    {
      PlayerBulletCount.Add(playerHashCode, 0);
    }
    if (PlayerBulletCount[playerHashCode] < MaxBulletsPerPerson)
    {
      GameObject bullet = Instantiate(BulletPrefab, launchPoint.position, Quaternion.identity);
      Bullet bulletScript = bullet.GetComponent<Bullet>();
      bulletScript.initialize(launchPoint.position, launchPoint.forward, playerHashCode);
      PlayerBulletCount[playerHashCode]++;
    }
  }

  void removeBulletFromPool(SignalData data)
  {
    Bullet bullet = data.get<Bullet>("Bullet");
    PlayerBulletCount[bullet.ParentPlayerHashCode]--;
    //Bullets.Remove(bullet.gameObject);
  }

}
