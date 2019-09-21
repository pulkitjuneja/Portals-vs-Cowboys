using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSetup : MonoBehaviour {
  PortalManager PortalManager;
  BulletManager BulletManager;
  public GameObject[] PlayerPrefabs;
  public GameObject PlayerUIPrefab;

  public PlayerSelectionData[] PlayerSelectionData;
  void Start() {
    PortalManager = this.GetComponent<PortalManager>();
    BulletManager = this.GetComponent<BulletManager>();
    SetPlayerData();
    SpawnPlayers();
    ActivateManagers();
  }

  // Temporary this data will come from the player selection menu
  void SetPlayerData() {
    PlayerSelectionData = new PlayerSelectionData[2];
    PlayerSelectionData[0] = new PlayerSelectionData(0, "Player1", 1, 0);
    PlayerSelectionData[1] = new PlayerSelectionData(1, "Player2", 2, 1);
  }

  void SpawnPlayers() {
    Dictionary<int, GameObject[]> spawnPoints = new Dictionary<int, GameObject[]>();
    spawnPoints.Add(0, GameObject.FindGameObjectsWithTag(Constants.Arena1PlayerSpawnPoint));
    spawnPoints.Add(1, GameObject.FindGameObjectsWithTag(Constants.Arena2PlayerSpawnPoint));
    int team1Counter = 0, team2Counter = 0;
    foreach (PlayerSelectionData data in PlayerSelectionData) {
      int playerCounter = data.ArenId == 0 ? team1Counter : team2Counter;
      Transform spawnPoint = spawnPoints[data.ArenId][0].transform;
      GameObject player = Instantiate(PlayerPrefabs[data.CharacterId], spawnPoint.position, spawnPoint.rotation);
      player.GetComponentInChildren<PlayerInput>().AssignButtons(data.ControllerId);
      player.GetComponentInChildren<PlayerShoot>().ArenaId = data.ArenId;
      player.GetComponentInChildren<PlayerHealth>().ArenaId = data.ArenId;
      // BindPlayerUI(data, playerCounter, player);
      if (data.ArenId == 0) {
        team1Counter++;
      } else {
        team2Counter++;
      }

    }
  }

  void BindPlayerUI(PlayerSelectionData data, int playerCounter, GameObject player) {
    string panelContainerString = data.ArenId == 0 ? "Arena1PlayerPanels" : "Arena2PlayerPanels";
    GameObject Panel = GameObject.Find(panelContainerString).transform.GetChild(playerCounter).gameObject;
    Panel.SetActive(true);
    Panel.GetComponentInChildren<PlayerUIController>().Initialize(player, data.Name, "Frank");
  }

  void ActivateManagers() {
    PortalManager.enabled = true;
    BulletManager.enabled = true;
  }
}
