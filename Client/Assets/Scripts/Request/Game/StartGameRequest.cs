using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class StartGameRequest : BaseRequest {
    private RoomPanel roomPanel;
    public override void Awake() {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.StartGame;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void OnResponse(string data) {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        if (returnCode == ReturnCode.Success) {
            facade.GetManager<UIManager>().PushPanelAsync(UIPanelType.Game);
            facade.EnterPlaying();
        } else {
            facade.GetManager<UIManager>().ShowMessage("oyun baslatılamadı");
        }
    }
}
