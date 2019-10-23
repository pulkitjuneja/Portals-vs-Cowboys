using System;
using UnityEngine;
using System.Collections.Generic;

public class TwoColorGradient : IEquatable<TwoColorGradient> {
  public Gradient PrimaryGradient;
  public Gradient TrailGradient;
  public Color PrimaryColor;

  public TwoColorGradient(Color primaryColor, Color accentColor) {
    PrimaryColor = primaryColor;
    CreatePrimaryGradient(primaryColor, accentColor);
    CreateTrailGradient(primaryColor, accentColor);
  }

  void CreatePrimaryGradient(Color primaryColor, Color accentColor) {
    GradientColorKey[] colorKey = new GradientColorKey[2];
    GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];

    colorKey[0].color = primaryColor;
    colorKey[0].time = 0.206f;
    colorKey[1].color = accentColor;
    colorKey[1].time = 0.647f;

    alphaKey[0].alpha = 1.0f;
    alphaKey[0].time = 0.794f;
    alphaKey[1].alpha = 0.0f;
    alphaKey[1].time = 1.0f;

    PrimaryGradient = new Gradient();
    PrimaryGradient.SetKeys(colorKey, alphaKey);
  }

  void CreateTrailGradient(Color primaryColor, Color accentColor) {
    GradientColorKey[] colorKey = new GradientColorKey[4];
    GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];

    colorKey[0].color = primaryColor;
    colorKey[0].time = 0.591f;
    colorKey[1].color = accentColor;
    colorKey[1].time = 0.838f;
    colorKey[1].color = accentColor;
    colorKey[1].time = 0.838f;

    alphaKey[0].alpha = 1.0f;
    alphaKey[0].time = 0.832f;
    alphaKey[1].alpha = 0.0f;
    alphaKey[1].time = 1.0f;

    TrailGradient = new Gradient();
    TrailGradient.SetKeys(colorKey, alphaKey);
  }

  public bool Equals(TwoColorGradient other) {
    if (other is null)
      return false;

    return this.PrimaryColor == other.PrimaryColor;
  }
  public override bool Equals(object obj) => Equals(obj as TwoColorGradient);
  public override int GetHashCode() => (PrimaryColor).GetHashCode();
}