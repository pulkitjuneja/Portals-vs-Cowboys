using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShoot : MonoBehaviour {
  public LineRenderer AimLine;
  public Transform BulletLaunchPoint;
  public Signal BulletSpawnSignal;
  public Signal PortalSpawnSignal;
  public float ShootInterval = 0.5f;
  public float PortalShootInterval = 0.5f;
  // TODO: come up with a better approach
  public int ArenaId;
  public TwoColorGradient CurrentPortalColors;

  public Action<int> UpdateHealthUIAction;
  public Action<Color> UpdatePortalUIAction;
  public BulletTypes BulletType;

  PlayerInput PlayerInput;
  float LastBulletLaunchTime;
  float LastPortalLaunchTime;
  RaycastHit FrontPointHit;
  bool FrontPointHitResult;

  int shootLayerMask;

  void Start() {
    PlayerInput = GetComponent<PlayerInput>();
    ChangeCurrentPortalColor(new TwoColorGradient(Color.clear, Color.clear));
    shootLayerMask = (1 << LayerMask.NameToLayer("Wall")) | (1 << LayerMask.NameToLayer("Portal"));
  }

  void Shoot() {
    if (Input.GetButtonDown(PlayerInput.Fire)) {
      float timeSinceLastLaunch = Time.time - LastBulletLaunchTime;
      if (timeSinceLastLaunch > ShootInterval) {
        SignalData spawnData = new SignalData();
        spawnData.set("launchPoint", BulletLaunchPoint);
        spawnData.set("playerHashCode", this.GetHashCode());
        spawnData.set("arenaId", ArenaId);
        spawnData.set("bulletType", BulletType);
        BulletSpawnSignal.fire(spawnData);
        LastBulletLaunchTime = Time.time;
      }
    }
  }

  public void ChangeCurrentPortalColor(TwoColorGradient portalColors) {
    CurrentPortalColors = portalColors;
    AimLine.startColor = AimLine.endColor = portalColors.PrimaryColor;
    //UpdatePortalUIAction(portalColors.PrimaryColor);
  }


  void ShootPortal() {
    if (Input.GetButtonDown(PlayerInput.FirePortal)) {
      float timeSinceLastLaunch = Time.time - LastPortalLaunchTime;
      if (timeSinceLastLaunch > PortalShootInterval &&
        !CurrentPortalColors.PrimaryColor.Equals(Color.clear)) {
        if (FrontPointHitResult) {
          if (FrontPointHit.transform.gameObject.layer == LayerMask.NameToLayer("Portal")) {
            return;
          }
          SignalData portalSpawnData = new SignalData();
          portalSpawnData.set("SpawnPosition", FrontPointHit.point);
          portalSpawnData.set("SpawnDirection", FrontPointHit.normal);
          portalSpawnData.set("ArenaId", ArenaId);
          portalSpawnData.set("PortalColors", CurrentPortalColors);
          PortalSpawnSignal.fire(portalSpawnData);
          LastPortalLaunchTime = Time.time;
          ChangeCurrentPortalColor(new TwoColorGradient(Color.clear, Color.clear));
        }
      }
    }
  }

  // calculating forward direction raycast hit here since it is used both by aim line renderer
  // and to shoot portals
  void CalculateFrontPointHit() {
    FrontPointHitResult = Physics.Raycast(BulletLaunchPoint.position, BulletLaunchPoint.forward,
     out FrontPointHit, Mathf.Infinity, shootLayerMask);
    AimLine.SetPositions(new Vector3[] { BulletLaunchPoint.position, FrontPointHit.point });
  }


  // Update is called once per frame
  void Update() {
    CalculateFrontPointHit();
    Shoot();
    ShootPortal();
  }
}
