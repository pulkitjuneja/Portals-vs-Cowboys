using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortalFluidSpawner : MonoBehaviour {
  public GameObject FluidPrefab;
  public Signal PortalFluidUsedSignal;
  public int RespawnInterval;

  Dictionary<int, Dictionary<Vector3, PortalColorData>> ArenaPointColorMap;
  void Start() {
    PortalFluidUsedSignal.addListener(onPortalFluidUse);
    ArenaPointColorMap = new Dictionary<int, Dictionary<Vector3, PortalColorData>>();
    SpawnInitialPortalFluids();
  }

  void SpawnInitialPortalFluids() {

    var arena1SpawnPoints = GameObject.FindGameObjectsWithTag(Constants.Arena1PortalFluidSpawnPoint);
    var arena2SpawnPoints = GameObject.FindGameObjectsWithTag(Constants.Arena2PortalFluidSpawnPoint);

    var shuffledColorList = GetShuffledColorList();

    // assuming arenas are symmetrical and both have same number of spawn points
    for (int i = 0; i < arena1SpawnPoints.Length; i++) {
      Vector3 positionArena1 = arena1SpawnPoints[i].transform.position;
      Vector3 positionArena2 = arena2SpawnPoints[i].transform.position;
      var portalColorData = shuffledColorList[i];
      Debug.Log(portalColorData);

      // instantiating in pairs so that both arenas have same colors in the starting
      SpawnAtPosition(positionArena1, portalColorData, 0);
      SpawnAtPosition(positionArena2, portalColorData, 1);
    }
  }

  void SpawnAtPosition(Vector3 position, PortalColorData portalColors, int arenaId) {
    GameObject portalFluidObject = Instantiate(FluidPrefab, position, Quaternion.identity);
    portalFluidObject.GetComponent<PortalFluid>().ResetProperties(portalColors, position, arenaId);
    if (!ArenaPointColorMap.ContainsKey(arenaId)) {
      ArenaPointColorMap.Add(arenaId, new Dictionary<Vector3, PortalColorData>());
    }
    ArenaPointColorMap[arenaId].Add(position, portalColors);
  }

  List<PortalColorData> GetShuffledColorList() {
    return Constants.portalColors.OrderBy(x => Random.value).ToList();
  }

  void onPortalFluidUse(SignalData fluidUSedData) {
    var arenaId = fluidUSedData.get<int>("arenaId");
    var position = fluidUSedData.get<Vector3>("location");
    var currentFluidColor = ArenaPointColorMap[arenaId][position];
    ArenaPointColorMap[arenaId].Remove(position);
    StartCoroutine(RespawnConsumedFluid(arenaId, position, currentFluidColor));
  }

  // Pick new color except the ones already present in the arena
  PortalColorData GetCurrentSubtractedColors(List<PortalColorData> currentMapColors, PortalColorData currentFluidColors) {
    var subtractedColors = Constants.portalColors.Except(currentMapColors).ToList();
    subtractedColors.Remove(currentFluidColors);
    if (subtractedColors.Count > 0) {
      return subtractedColors[Random.Range(0, subtractedColors.Count - 1)];
    } else {
      return Constants.portalColors[Random.Range(0, Constants.portalColors.Count - 1)];
    }
  }

  IEnumerator RespawnConsumedFluid(int arenaId, Vector3 location, PortalColorData currentFluidColors) {
    yield return new WaitForSeconds(RespawnInterval);
    var currentMap = ArenaPointColorMap[arenaId];
    var otherMap = ArenaPointColorMap[1 - arenaId];
    var currentMapColors = currentMap.Values.ToList();
    var otherMapColors = otherMap.Values.ToList();
    float probability = Random.value;
    PortalColorData newColor;
    if (probability > 0.6) {
      newColor = GetCurrentSubtractedColors(currentMapColors, currentFluidColors);
    } else {
      Debug.Log("Other subtraction");
      var subtractedColors = otherMapColors.Except(currentMapColors).ToList();
      if (subtractedColors.Count > 0) {
        newColor = subtractedColors[Random.Range(0, subtractedColors.Count - 1)];
      } else {
        newColor = GetCurrentSubtractedColors(currentMapColors, currentFluidColors);
      }
    }
    SpawnAtPosition(location, newColor, arenaId);
  }
}
