using System;
using System.Collections.Generic;
using UnityEngine;


public static class Constants {
  public static List<TwoColorGradient> portalColors = new List<TwoColorGradient>
  {
     new TwoColorGradient(new Color(0,1,0.0705f), new Color(0,0.25f,0.0186f)),
     new TwoColorGradient(new Color(1,1,0), new Color(0.5849f,0.18404f,0)),
     new TwoColorGradient(new Color(0,0.9520f,1), new Color(0.03529f,0.32549f,1)),
     new TwoColorGradient(new Color(1.333333f,0,2), new Color(0.25f,0,0.25f)),
     new TwoColorGradient(new Color(2.670157f,2.670157f,2.670157f), new Color(0.5f,0.5f,0.5f)),
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
  public const string Arena1PortalFluidSpawnPoint = "Arena1PortalFluidSpawnPoint";
  public const string Arena2PortalFluidSpawnPoint = "Arena2PortalFluidSpawnPoint";
  public const string Projectiles = "Projectiles";
}