using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

  public Image Playerhealth;
  public Image PortalFluidColor;
  public Text PlayerName;

  public Text CharacterName;

  public Sprite[] HealthSprites;

  public void Initialize(GameObject player, string playerName, string characterName) {
    PlayerName.text = playerName + "/" + characterName;
    player.GetComponent<PlayerShoot>().UpdateHealthUIAction = updateHealthImage;
    player.GetComponent<PlayerShoot>().UpdatePortalUIAction = updatePortalColorUI;
  }

  void updateHealthImage(int health) {
    Playerhealth.sprite = HealthSprites[health - 1];
  }

  void updatePortalColorUI(Color color) {
    PortalFluidColor.color = color;
  }

  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }
}
