using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class MoveRequest : BaseRequest {

    private Transform localPlayerTransform;
    private PlayerMove localPlayerMove;
    private int syncRate = 30;

    private Transform remotePlayerTransform;
    private Animator remotePlayerAnim;
    private bool isSyncRemotePlayer = false;
    private Vector3 pos;
    private Vector3 rotation;
    private float forward;
    public override void Awake() {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }

    private void Start() {
        InvokeRepeating("SyncLocalPlayer", 1f, 1f / syncRate);
    }
    private void FixedUpdate() {
        if (isSyncRemotePlayer) {
            SyncRemotePlayer();
            isSyncRemotePlayer = false;
        }
    }
    public MoveRequest SetLocalPlayer(GameObject go) {
        this.localPlayerTransform = go.transform;
        this.localPlayerMove = go.GetComponent<PlayerMove>();
        return this;
    }
    public MoveRequest SetRemotePlayer(GameObject remotePlayer) {
        this.remotePlayerTransform = remotePlayer.transform;
        this.remotePlayerAnim = remotePlayerTransform.GetComponent<Animator>();
        return this;
    }
    public void SetPlayers(GameObject localPlayer, GameObject remotePlayer) {
        SetLocalPlayer(localPlayer);
        SetRemotePlayer(remotePlayer);
    }
    
    private void SyncLocalPlayer() {
        SendRequest(localPlayerTransform.position, localPlayerTransform.eulerAngles, localPlayerMove.forward);
    }
    private void SyncRemotePlayer() {
        remotePlayerTransform.position = pos;
        remotePlayerTransform.eulerAngles = rotation;
        remotePlayerAnim.SetFloat("Forward", forward);
    }

  
    private void SendRequest(Vector3 pos, Vector3 rot, float forward) {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}"
            , pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, forward);
        base.SendRequest(data);
    }

    public override void OnResponse(string data) {//27.75,0,1.41-0,0,0-0
        string[] strs = data.Split('|');
        pos = UnityTools.ParseVector3(strs[0]);
        rotation = UnityTools.ParseVector3(strs[1]);
        forward = float.Parse(strs[2]);
        isSyncRemotePlayer = true;
    }

}
