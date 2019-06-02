using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PortalManager : MonoBehaviour
{
  public Signal PortalTimeoutSignal;
  public Transform level;
  Dictionary<Transform, bool> PortalOneSpawnPoints, PortalTwoSpawnPoints;
  public GameObject portalPrefab;

  public int spawnInterval = 3;

  void Start()
  {
    initializeSpawnPoints();
    StartCoroutine(startSpawning());
    PortalTimeoutSignal.addListener(onPortalTimeOut);
  }

  void initializeSpawnPoints()
  {
    PortalOneSpawnPoints = new Dictionary<Transform, bool>();
    PortalTwoSpawnPoints = new Dictionary<Transform, bool>();
    Transform spawnPointHodler = level.Find("PortalSpawnPoints");
    foreach (Transform child in spawnPointHodler)
    {
      switch (child.tag)
      {
        case Tags.PortalOneSpawn:
          PortalOneSpawnPoints.Add(child, false);
          break;
        case Tags.PortalTwoSpawn:
          PortalTwoSpawnPoints.Add(child, false);
          break;
        default: break;
      }
    }
  }

  IEnumerator startSpawning()
  {
    while (true)
    {
      yield return new WaitForSeconds(spawnInterval);
      spawnPortals();
    }
  }

  Transform getEmptySpawnPoint(Dictionary<Transform, bool> pointArray)
  {
    int iterationCount = pointArray.Count * 2;
    List<Transform> keys = new List<Transform>(pointArray.Keys);
    Transform key = null;
    do
    {
      key = keys[UnityEngine.Random.Range(0, keys.Count - 1)];
      iterationCount--;
    } while (pointArray[key] && iterationCount > 0);
    if (!pointArray[key])
    {
      pointArray[key] = true;
      return key;
    }
    return null;
  }


  void spawnPortals()
  {
    var point1 = getEmptySpawnPoint(PortalOneSpawnPoints);
    var point2 = getEmptySpawnPoint(PortalTwoSpawnPoints);
    if (point1 == null || point2 == null)
    {
      return;
    }
    GameObject portal1 = Instantiate(portalPrefab, point1.position, point1.rotation);
    GameObject portal2 = Instantiate(portalPrefab, point2.position, point2.rotation);
    UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
    var portalColor = new Color(
      UnityEngine.Random.Range(0.2f, 1.1f),
      UnityEngine.Random.Range(0.0f, 1.1f),
      UnityEngine.Random.Range(0.0f, 1.1f),
      1.2f
    );
    portal1.GetComponent<Portal>().initialize(portalColor, portal2, 1, point1);
    portal2.GetComponent<Portal>().initialize(portalColor, portal1, 2, point2);
  }

  void onPortalTimeOut(SignalData data)
  {
    int playerId = data.get<int>("PlayerId");
    Transform point = data.get<Transform>("SourcePoint");
    if (playerId == 1)
    {
      PortalOneSpawnPoints[point] = false;
    }
    else
    {
      PortalTwoSpawnPoints[point] = false;
    }
  }

}
