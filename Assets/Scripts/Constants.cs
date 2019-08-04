using System;
using System.Collections.Generic;
using UnityEngine;


public static class Constants {
  public static List<Color> portalColors = new List<Color>
  {
     new Color(1.2f,0,0),
     new Color(0,1.2f,0),
     new Color(0,0,1.2f)
  };

  public static List<Vector2> PlayerPanelPositions = new List<Vector2>
  {
    new Vector2 (9,-4.6f),
    new Vector2 (9,-665.7f),
    new Vector2 (9,-4.6f),
    new Vector2 (9,-665.7f)
  };

  //Tags
  public const string Arena1PlayerSpawnPoint = "Arena1PlayerSpawnPoint";
  public const string Arena2PlayerSpawnPoint = "Arena2PlayerSpawnPoint";
  public const string PortalOneSpawn = "PortalOneSpawn";
  public const string PortalTwoSpawn = "PortalTwoSpawn";
  public const string Projectiles = "Projectiles";
}