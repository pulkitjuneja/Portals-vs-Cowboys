using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AnnouncementTypes {
  RoundStart,
  Draw,
  Win
}

public class AnnouncementController : MonoBehaviour {

  public Animator AnnouncementParent;
  public Text AnouncementText;

  public Signal ShowAnouncementSignal;

  void Start() {
    ShowAnouncementSignal.addListener(ShowAnnouncement);
  }

  void ShowAnnouncement(SignalData data) {
    AnnouncementTypes announcementType = data.get<AnnouncementTypes>("AnnouncementType");
    switch (announcementType) {
      case AnnouncementTypes.RoundStart: {
          int roundNumber = data.get<int>("RoundNumber");
          StartCoroutine(ShowRoundStart(roundNumber));
          break;
        }
      case AnnouncementTypes.Draw: {
          AnouncementText.text = "Its a Draw !";
          AnnouncementParent.SetTrigger("ShowAnnouncement");
          break;
        }
      case AnnouncementTypes.Win: {
          int winningTeamId = data.get<int>("WinningTeamId");
          AnouncementText.text = "Team " + winningTeamId.ToString() + " Wins !";
          AnnouncementParent.SetTrigger("ShowAnnouncement");
          break;
        }
    }
  }

  IEnumerator ShowRoundStart(int roundNumber) {
    AnouncementText.text = "Round " + roundNumber.ToString();
    AnnouncementParent.SetTrigger("ShowAnnouncement");
    yield return new WaitForSeconds(1.0f);
    AnouncementText.text = "Begin !";
    yield return new WaitForSeconds(1.0f);
  }
}
