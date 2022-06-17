using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public GameObject bulletPrefab;
    private Transform leftHandTrans;
    private Vector3 shootDir;
    private PlayerManager playerMng;

	void Start () {
        leftHandTrans = transform.Find("point");
	}

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool isCollider = Physics.Raycast(ray, out hit);
            if (isCollider)
            {
                Vector3 targetPoint = hit.point;
                targetPoint.y = transform.position.y;
                shootDir = targetPoint - transform.position;
                transform.rotation = Quaternion.LookRotation(shootDir);
                Invoke("Shoot", 0.1f);
            }
        }

    }
    public void SetPlayerMng(PlayerManager playerMng)
    {
        this.playerMng = playerMng;
    }


    private void Shoot() {
        playerMng.Shoot(bulletPrefab, leftHandTrans.position, Quaternion.LookRotation(shootDir));
    }
}
