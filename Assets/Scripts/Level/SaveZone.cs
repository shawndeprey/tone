using UnityEngine;

public class SaveZone : SpecialZone
{
    protected override void DoZoneAction()
    {
        MenuManager.Instance.SaveIndicator();
        GameManager.Instance.SaveGame(1);
    }
}