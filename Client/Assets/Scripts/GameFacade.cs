using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;
using UnityEngine.UI;

public enum ManagerType {
    None,
    UI,
    Audio,
    Player,
    Camera,
    Request,
    Client,
}

/// 1. Synchronize the entire game process to all instances
/// 2. Manage all Managers
/// 3. Access mediation for most instances in the game system
public class GameFacade : MonoBehaviour {

    private static GameFacade _instance = null;
    public static GameFacade Instance {
        get {
            if (_instance == null) {
                GameObject go = GameObject.Find("GameFacade");
                if (go != null) {
                    _instance = go.GetComponent<GameFacade>();
                }
            }
            return _instance;
        }
    }

    public Dictionary<ManagerType, BaseManager> managerDict = new Dictionary<ManagerType, BaseManager>();
    public Manager GetManager<Manager>() where Manager:BaseManager{
        ManagerType mt = ScriptExtends.TypeToEnum<ManagerType, Manager>("Manager");
        return (Manager)managerDict[mt];
    }

    private bool isEnterPlaying = false;


 
    void Start() {
        InitManager();
        foreach (var value in managerDict.Values) {value.OnInit();}
        
        GetManager<UIManager>().PushPanel(UIPanelType.Start);
    }
    void Update() {
        foreach (var value in managerDict.Values) { value.Update(); }

        if (isEnterPlaying) {
            GetManager<PlayerManager>().SpawnRoles();
            GetManager<CameraManager>().FollowRole();
            isEnterPlaying = false;
        }
    }
    private void OnDestroy() {
        foreach (var value in managerDict.Values) { value.OnDestroy(); }
    }

    private void InitManager() {
        foreach (var value in Enum.GetValues(typeof(ManagerType))) {
            if ((ManagerType)value != ManagerType.None) {
                managerDict.Add((ManagerType)value,
                    (BaseManager)Activator.CreateInstance(
                        Type.GetType(value.ToString() + "Manager"), new object[] { this }));
            }
        }
    }



    public void AddRequest(ActionCode actionCode, BaseRequest request) {
        GetManager<RequestManager>().AddRequest(actionCode, request);
    }
    public void RemoveRequest(ActionCode actionCode) {
        GetManager<RequestManager>().RemoveRequest(actionCode);
    }
    public void HandleReponse(ActionCode actionCode, string data) {
        GetManager<RequestManager>().HandleReponse(actionCode, data);
    }

    public void ShowMessage(string msg) {
        GetManager<UIManager>().ShowMessage(msg);
    }

    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data) {
        GetManager<ClientManager>().SendRequest(requestCode, actionCode, data);
    }

    public void PlayBgSound(string soundName) {
        GetManager<AudioManager>().PlayBgSound(soundName);
    }
    public void PlayNormalSound(string soundName) {
        GetManager<AudioManager>().PlayNormalSound(soundName);
    }

    public void SetUserData(UserData ud) {
        GetManager<PlayerManager>().UserData = ud;
    }
    public UserData GetUserData() {
        return GetManager<PlayerManager>().UserData;
    }
    public void SetCurrentRoleType(RoleType rt) {
        GetManager<PlayerManager>().SetCurrentRoleType(rt);
    }
    public GameObject GetCurrentRoleGameObject() {
        return GetManager<PlayerManager>().GetCurrentRoleGameObject();
    }



    public void EnterPlaying() {
        isEnterPlaying = true;
    }
    public void StartPlaying() {
        GetManager<PlayerManager>().AddControlScript();
        GetManager<PlayerManager>().CreateSyncRequest();
    }
    public void SendAttack(RoleType rt, int damage) {
        GetManager<PlayerManager>().SendAttack(rt,damage);
    }
    public void GameOver() {
        GetManager<CameraManager>().WalkthroughScene();
        GetManager<PlayerManager>().GameOver();
    }
    public void UpdateResult(int totalCount, int winCount) {
        GetManager<PlayerManager>().UpdateResult(totalCount, winCount);
    }
}
