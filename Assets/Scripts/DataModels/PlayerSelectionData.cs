using System;
using System.Collections.Generic;

// Static class to hold player selection Data between scene changes
public class PlayerSelectionData {
  public int CharacterId;
  public string Name;
  public int ControllerId;
  public int ArenId;

  public PlayerSelectionData(int characterId, string name, int controllerId, int arenaID) {
    CharacterId = characterId;
    Name = name;
    ControllerId = controllerId;
    ArenId = arenaID;
  }

}