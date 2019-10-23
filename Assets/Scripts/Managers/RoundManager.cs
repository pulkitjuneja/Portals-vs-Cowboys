using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoundManager : MonoBehaviour {

  public Signal PlayerKiledSignal;
  public Signal RoundTimeoutSignal;
  public Signal ShowAnouncementSignal;
  public Text RoundTimerText;
  public PlayerSelectionData[] PlayerSelectionData;
  public PortalManager PortalManager;
  public BulletManager BulletManager;

  public GameObject[] PlayerPrefabs;

  public PortalFluidSpawner PortalFluidManager;

  int[] PerArenaPLayerCount;
  float RoundTime;
  const float MaxRoundTIme = 240;
  SimpleStateMachine RoundStates;

  void Start() {
    PlayerKiledSignal.addListener(OnPlayerKilled);
    SetPlayerData();
    RoundStates = new SimpleStateMachine(BeforeRound(1), this);
    StartCoroutine(RoundStates.Start());
  }

  void SetPlayerData() {
    PlayerSelectionData = new PlayerSelectionData[2];
    PlayerSelectionData[0] = new PlayerSelectionData(0, "Player1", 1, 0);
    PlayerSelectionData[1] = new PlayerSelectionData(1, "Player2", 2, 1);
  }

  IEnumerator BeforeRound(int roundId) {
    SignalData showAnnouncementData = new SignalData();
    showAnnouncementData.set("AnnouncementType", AnnouncementTypes.RoundStart);
    showAnnouncementData.set("RoundNumber", roundId);
    ShowAnouncementSignal.fire(showAnnouncementData);
    yield return new WaitForSeconds(2.0f);
    RoundTime = MaxRoundTIme;
    ToggleManagers(true);
    SpawnPlayers();
    RoundStates.SetState(RoundUpdate());
  }

  IEnumerator AfterRound(int winningTeamId) {
    ToggleManagers(false);
    SignalData showAnnouncementData = new SignalData();
    if (winningTeamId == -1) {
      showAnnouncementData.set("AnnouncementType", AnnouncementTypes.Win);
    } else {
      showAnnouncementData.set("AnnouncementType", AnnouncementTypes.Win);
      showAnnouncementData.set("WinningTeamId", winningTeamId);
    }
    yield return null;
  }

  IEnumerator RoundUpdate() {
    while (RoundTime > 0) {
      RoundTime -= Time.deltaTime;
      SetTimerText();
      yield return null;
    }
    int winningTeamId = FindWinnerOnTimeOut();
    RoundStates.SetState(AfterRound(winningTeamId));
  }

  void OnPlayerKilled(SignalData data) {
    int ArenaId = data.get<int>("ArenaId");
    PerArenaPLayerCount[ArenaId]--;
    if (PerArenaPLayerCount[ArenaId] <= 0) {
      int winningTeamId = 1 - ArenaId;
      IEnumerator newState = AfterRound(winningTeamId);
    }
  }

  int FindWinnerOnTimeOut() {
    if (PerArenaPLayerCount[0] > PerArenaPLayerCount[1])
      return 0;
    else if (PerArenaPLayerCount[0] == PerArenaPLayerCount[1])
      return -1;
    else return 1;
  }

  void SetTimerText() {
    string minutes = ((int)RoundTime / 60).ToString();
    string seconds = (RoundTime % 60).ToString("f0");
    RoundTimerText.text = minutes + " : " + seconds;
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
      if (data.ArenId == 0) {
        team1Counter++;
      } else {
        team2Counter++;
      }

    }
  }

  void ToggleManagers(bool enabled) {
    PortalManager.enabled = enabled;
    BulletManager.enabled = enabled;
    PortalFluidSpawner.enabled = enabled;
  }

}