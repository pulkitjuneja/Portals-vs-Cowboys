using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSetup : MonoBehaviour {
  PortalManager PortalManager;
  BulletManager BulletManager;
  public GameObject PlayerPrefab;
  void Start() {
    PortalManager = this.GetComponent<PortalManager>();
    BulletManager = this.GetComponent<BulletManager>();
    SpawnPlayers();
    ActivateManagers();
  }

  void SpawnPlayers() {
    Transform playerOneSpawnPoint = GameObject.FindWithTag(Tags.PlayerOneSpawn).transform;
    Transform playerTwoSpawnPoint = GameObject.FindWithTag(Tags.PlayerTwoSpawn).transform;
    GameObject playerOne = Instantiate(PlayerPrefab, playerOneSpawnPoint.position, playerOneSpawnPoint.rotation);
    playerOne.GetComponent<PlayerInput>().AssignButtons(1);
    playerOne.GetComponent<PlayerShoot>().ArenaId = 0;
    GameObject playerTwo = Instantiate(PlayerPrefab, playerTwoSpawnPoint.position, playerTwoSpawnPoint.rotation);
    playerTwo.GetComponent<PlayerInput>().AssignButtons(2);
    playerTwo.GetComponent<PlayerShoot>().ArenaId = 1;
  }

  void ActivateManagers() {
    PortalManager.enabled = true;
    BulletManager.enabled = true;
  }
}
