using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PortalManager : MonoBehaviour {
  public Signal PortalTimeoutSignal;
  public Signal PortalSpawnSignal;
  public GameObject portalPrefab;
  // public int spawnInterval = 3;

  public Dictionary<Color, Dictionary<int, GameObject>> PortalMap;

  void Start() {
    // initializeSpawnPoints();
    // StartCoroutine(startSpawning());
    PortalTimeoutSignal.addListener(onPortalTimeOut);
    PortalMap = new Dictionary<Color, Dictionary<int, GameObject>>();
    PortalSpawnSignal.addListener(SpawnPortal);
  }

  void SpawnPortal(SignalData portalSpawnData) {
    Vector3 position = portalSpawnData.get<Vector3>("SpawnPosition");
    Vector3 direction = portalSpawnData.get<Vector3>("SpawnDirection");
    Color portalColor = portalSpawnData.get<Color>("PortalColor");
    int arenaId = portalSpawnData.get<int>("ArenaId");
    Debug.Log(arenaId);
    var spawnPosition = new Vector3(position.x, 1.4f, position.z) + direction * 0.01f;
    Quaternion portalRotation = Quaternion.LookRotation(direction);
    GameObject portalObject = Instantiate(portalPrefab, spawnPosition, portalRotation);
    GameObject correspondingPortal = null;
    if (PortalMap.ContainsKey(portalColor)) {
      if (PortalMap[portalColor].ContainsKey(arenaId)) {
        Destroy(PortalMap[portalColor][arenaId]);
        PortalMap[portalColor].Remove(arenaId);
      }
      correspondingPortal = getCorrespondingPortal(arenaId, portalColor);
    } else {
      PortalMap.Add(portalColor, new Dictionary<int, GameObject>());
    }
    if (correspondingPortal != null) {
      correspondingPortal.GetComponent<Portal>().changeCorrespondingPortal(portalObject);
    }
    portalObject.GetComponent<Portal>().initialize(portalColor, correspondingPortal, arenaId);
    PortalMap[portalColor].Add(arenaId, portalObject);
  }

  void onPortalTimeOut(SignalData data) {
    int arenaId = data.get<int>("arenaId");
    Color portalColor = data.get<Color>("portalColor");
    GameObject otherPortal = getCorrespondingPortal(arenaId, portalColor);
    if (otherPortal != null) {
      otherPortal.GetComponent<Portal>().changeCorrespondingPortal(null);
    }
    PortalMap[portalColor].Remove(arenaId);
    Debug.Log("Portal map");
  }

  GameObject getCorrespondingPortal(int arenaId, Color portalColor) {
    int otherPortalId = 1 - arenaId;
    if (PortalMap.ContainsKey(portalColor) && PortalMap[portalColor].ContainsKey(otherPortalId)) {
      return PortalMap[portalColor][otherPortalId];
    }
    return null;
  }


  // void initializeSpawnPoints()
  // {
  //   PortalOneSpawnPoints = new Dictionary<Transform, bool>();
  //   PortalTwoSpawnPoints = new Dictionary<Transform, bool>();
  //   Transform spawnPointHodler = level.Find("PortalSpawnPoints");
  //   foreach (Transform child in spawnPointHodler)
  //   {
  //     switch (child.tag)
  //     {
  //       case Tags.PortalOneSpawn:
  //         PortalOneSpawnPoints.Add(child, false);
  //         break;
  //       case Tags.PortalTwoSpawn:
  //         PortalTwoSpawnPoints.Add(child, false);
  //         break;
  //       default: break;
  //     }
  //   }
  // }

  // IEnumerator startSpawning() {
  //   while (true) {
  //     yield return new WaitForSeconds(spawnInterval);
  //     spawnPortals();
  //   }
  // }

  // Transform getEmptySpawnPoint(Dictionary<Transform, bool> pointArray) {
  //   int iterationCount = pointArray.Count * 2;
  //   List<Transform> keys = new List<Transform>(pointArray.Keys);
  //   Transform key = null;
  //   do {
  //     key = keys[UnityEngine.Random.Range(0, keys.Count - 1)];
  //     iterationCount--;
  //   } while (pointArray[key] && iterationCount > 0);
  //   if (!pointArray[key]) {
  //     pointArray[key] = true;
  //     return key;
  //   }
  //   return null;
  // }


  // void spawnPortals() {
  //   var point1 = getEmptySpawnPoint(PortalOneSpawnPoints);
  //   var point2 = getEmptySpawnPoint(PortalTwoSpawnPoints);
  //   if (point1 == null || point2 == null) {
  //     return;
  //   }
  //   GameObject portal1 = Instantiate(portalPrefab, point1.position, point1.rotation);
  //   GameObject portal2 = Instantiate(portalPrefab, point2.position, point2.rotation);
  //   UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
  //   var portalColor = new Color(
  //     UnityEngine.Random.Range(0.2f, 1.1f),
  //     UnityEngine.Random.Range(0.0f, 1.1f),
  //     UnityEngine.Random.Range(0.0f, 1.1f),
  //     1.2f
  //   );
  //   portal1.GetComponent<Portal>().initialize(portalColor, portal2, 1, point1);
  //   portal2.GetComponent<Portal>().initialize(portalColor, portal1, 2, point2);
  // }

}
