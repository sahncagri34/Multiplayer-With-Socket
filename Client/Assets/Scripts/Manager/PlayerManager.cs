using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class PlayerManager : BaseManager {
    public PlayerManager(GameFacade facade) : base(facade) { }

    private UserData userData;
    private Dictionary<RoleType, RoleData> roleDataDict 
        = new Dictionary<RoleType, RoleData>();

    private Transform rolePositions;

    private RoleType currentRoleType;
    private GameObject currentRoleGameObject;
    private GameObject remoteRoleGameObject;

    private GameObject playerSync;

    public void UpdateResult(int totalCount, int winCount) {
        userData.TotalCount = totalCount;
        userData.WinCount = winCount;
    }
    public void SetCurrentRoleType(RoleType rt) {
        currentRoleType = rt;
    }
    public UserData UserData {
        set { userData = value; }
        get { return userData; }
    }
    public override void OnInit() {
        rolePositions = GameObject.Find("RolePositions").transform;
        InitRoleDataDict();
    }

    private void InitRoleDataDict() {
        roleDataDict.Add(RoleType.Blue, new RoleData(RoleType.Blue, "Hunter_BLUE", "Bullet_BLUE", "Explosion_BLUE", rolePositions.Find("Position1")));
        roleDataDict.Add(RoleType.Red, new RoleData(RoleType.Red, "Hunter_RED", "Bullet_RED", "Explosion_RED", rolePositions.Find("Position2")));
    }
    public void SpawnRoles() {
        foreach (RoleData rd in roleDataDict.Values) {
            GameObject go = GameObject.Instantiate(rd.RolePrefab, rd.SpawnPosition, Quaternion.identity);
            go.tag = "Player";
            if (rd.RoleType == currentRoleType) {
                currentRoleGameObject = go;
                currentRoleGameObject.GetComponent<PlayerInfo>().isLocal = true;
            } else {
                remoteRoleGameObject = go;
            }
        }
    }
    public GameObject GetCurrentRoleGameObject() {
        return currentRoleGameObject;
    }
    private RoleData GetRoleData(RoleType rt) {
        RoleData rd = null;
        roleDataDict.TryGetValue(rt, out rd);
        return rd;
    }

 
    public void AddControlScript() {
        currentRoleGameObject.AddComponent<PlayerMove>();
        RoleType rt = currentRoleGameObject.GetComponent<PlayerInfo>().roleType;
        RoleData rd = GetRoleData(rt);

        PlayerAttack playerAttack = currentRoleGameObject.AddComponent<PlayerAttack>();
        playerAttack.bulletPrefab = rd.BulletPrefab;
        playerAttack.SetPlayerMng(this);
    }

 
    public void CreateSyncRequest() {
        playerSync = new GameObject("PlayerSyncRequest");

        playerSync.AddComponent<MoveRequest>().SetPlayers(currentRoleGameObject, remoteRoleGameObject);
        playerSync.AddComponent<ShootRequest>().playerMng = this;
        playerSync.AddComponent<AttackRequest>();
    }


    public void Shoot(GameObject bulletPrefab, Vector3 pos, Quaternion rotation) {
        facade.PlayNormalSound(AudioManager.Sound_Timer);
        GameObject.Instantiate(bulletPrefab, pos, rotation).GetComponent<Bullet>().isLocal = true;
        playerSync.GetComponent<ShootRequest>().
            SendRequest(bulletPrefab.GetComponent<Bullet>().roleType, pos, rotation.eulerAngles);
    }

    public void RemoteShoot(RoleType rt, Vector3 pos, Vector3 rotation) {
        GameObject bulletPrefab = GetRoleData(rt).BulletPrefab;
        Transform transform = GameObject.Instantiate(bulletPrefab).GetComponent<Transform>();
        transform.position = pos;
        transform.eulerAngles = rotation;
    }

    public void SendAttack(RoleType rt, int damage) {
        playerSync.GetComponent<AttackRequest>() .SendRequest(rt, damage);
    }

    public void GameOver() {
        GameObject.Destroy(playerSync);
        GameObject.Destroy(currentRoleGameObject);
        GameObject.Destroy(remoteRoleGameObject);
    }
}
