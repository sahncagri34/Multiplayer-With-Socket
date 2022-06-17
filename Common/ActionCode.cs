using System;
using System.Collections.Generic;
using System.Text;

namespace Common {
    public enum ActionCode {
        None,
        Login,
        Register,
        ListRoom,
        CreateRoom,
        JoinRoom,
        UpdateRoom,
        QuitRoom,
        StartGame,
        ShowTimer,
        StartPlay,
        Move,
        Shoot,
        Attack,
        GameOver,
        UpdateResult,
        QuitBattle,
        SendMessage
    }
}
// DATA =>  RequestCode + ActionCode + s

//  RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 4);
//  ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 8);
//  string s = Encoding.UTF8.GetString(data, 12, count - 8);

//LOGIN VE REGISTER
// RequstCode+ActionCode+"username,password"

//   public void SendRequest(string username, string password) {
//         string data = username + "," + password;
//         base.SendRequest(data);
//     }


//   protected void SendRequest(string data) {
//         facade.SendRequest(requestCode, actionCode, data);
//     }

//CHAT
// public override void SendRequest()
//     {
//         UserData ud = facade.GetUserData();
//         string data = "Player "+ud.Username+" : "+messageInput.text;
//         messageInput.text = "";
//         base.SendRequest(data);
//     }

// ROOM JOİN
//  public void SendRequest(int roomId) {
//         base.SendRequest(roomId.ToString());
//     }

//ATTACK REQUEST
//  public void SendRequest(RoleType rt,int damage) {
//         base.SendRequest(rt + "," + damage.ToString());
//     }


//MOVE REQUEST
// private void SendRequest(Vector3 pos, Vector3 rot, float forward) {
//         string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}"
//             , pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, forward);
//         base.SendRequest(data);
//     }

//SHOOT REQUEST
//  public void SendRequest(RoleType rt, Vector3 pos, Vector3 rotation) {
//         string data = string.Format("{0}|{1},{2},{3}|{4},{5},{6}", (int)rt, pos.x, pos.y, pos.z, rotation.x, rotation.y, rotation.z);
//         base.SendRequest(data);
//     }