using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RoleData
{
    private const string PREFIX_PREFAB = "Prefabs/";

    public RoleType RoleType { get; private set; }
    public GameObject RolePrefab { get; private set; }
    public GameObject BulletPrefab { get; private set; }
    public Vector3 SpawnPosition { get; private set; }
    public GameObject ExplostionEffect { get; private set; }

    public RoleData(RoleType roleType,string rolePath,string bulletPath,string explosionPath, Transform spawnPos)
    {
        this.RoleType = roleType;
        this.RolePrefab = Resources.Load(PREFIX_PREFAB+ rolePath) as GameObject;
        this.BulletPrefab = Resources.Load(PREFIX_PREFAB + bulletPath) as GameObject;
        this.ExplostionEffect = Resources.Load(PREFIX_PREFAB + explosionPath) as GameObject;
        BulletPrefab.GetComponent<Bullet>().explosionEffect = ExplostionEffect;
        this.SpawnPosition = spawnPos.position;
    }
}
