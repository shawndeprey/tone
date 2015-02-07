using UnityEngine;
using System.Collections;

public class Door : SpecialZone
{
    public string sceneName;
    public string doorName;

    protected override void DoZoneAction()
    {
        GameManager.Instance.SetDoor(doorName);
        Application.LoadLevel(sceneName);
    }
}
