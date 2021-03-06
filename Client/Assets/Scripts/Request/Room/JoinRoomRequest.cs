using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class JoinRoomRequest : BaseRequest {

    private RoomListPanel roomListPanel;

    public override void Awake() {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public void SendRequest(int id) {
        base.SendRequest(id.ToString());
    }


    public override void OnResponse(string data) {
        string[] strs = data.Split('-');
        string[] strs2 = strs[0].Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs2[0]);
        UserData ud1 = null;
        UserData ud2 = null;

        switch (returnCode) {
            case ReturnCode.NotFound:
                facade.ShowMessage("Not found");
                break;
            case ReturnCode.Fail:
                facade.ShowMessage("fail");
                break;
            case ReturnCode.Success:
                string[] udStrArray = strs[1].Split('|');
                ud1 = new UserData(udStrArray[0]);
                ud2 = new UserData(udStrArray[1]);

                RoleType roleType = (RoleType)int.Parse(strs2[1]);
                facade.SetCurrentRoleType(roleType);

                UIManager um = facade.GetManager<UIManager>();
                um.ud1 = ud1;
                um.ud2 = ud2;

                um.PushPanelAsync(UIPanelType.Room);
                break;
        }
    }
}
