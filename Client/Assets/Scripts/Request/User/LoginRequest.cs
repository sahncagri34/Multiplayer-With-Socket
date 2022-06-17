﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class LoginRequest : BaseRequest {
    private LoginPanel loginPanel;
    // Use this for initialization
    public override void Awake() {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        loginPanel = GetComponent<LoginPanel>();
        base.Awake();
    }
    public void SendRequest(string username, string password) {
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data) {
        Debug.Log(data);
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);

        if (returnCode == ReturnCode.Success) {
            string username = strs[1];
            int totalCount = int.Parse(strs[2]);
            int winCount = int.Parse(strs[3]);
            UserData ud = new UserData(username, totalCount, winCount);
            facade.SetUserData(ud);

            facade.GetManager<UIManager>().PushPanelAsync(UIPanelType.RoomList);
        } else {
            facade.GetManager<UIManager>().ShowMessage("giriş yapılamadı");
        }
    }

}
