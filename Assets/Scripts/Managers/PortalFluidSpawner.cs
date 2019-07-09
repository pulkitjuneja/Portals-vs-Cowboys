using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFluidSpawner : MonoBehaviour {
  public GameObject FluidPrefab;
  Dictionary<Vector3, Color> Arena1PointColorMap;
  Dictionary<Vector3, Color> Arena2PointColorMap;
  void Start() {
    Arena1PointColorMap = new Dictionary<Vector3, Color>();
    Arena2PointColorMap = new Dictionary<Vector3, Color>();
    var arena1SpawnPoints = GameObject.FindGameObjectWithTag("Arena1PortalFluidSpawnPoint");
    var arena2SpawnPoints = GameObject.FindGameObjectWithTag("Arena2PortalFluidSpawnPoint");
    foreach (GameObject point in arena1SpawnPoints) {
      PortalFluidSpawner = Instantiate(FluidPrefab, point, Quaternion.identity);
    }

  }

  // Update is called once per frame
  void Update() {

  }
}
