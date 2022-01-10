using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class R2_Trigger1_Enter : MonoBehaviourPunCallbacks
{
    public BulletScript bullet;
    

    void Update()
    {

    }
    /*��� Ʈ���ſ� ���Դ��� üũ*/
    private bool AllInTrigger()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].transform.GetComponent<PlayerScript>().IsRound2_Trigger)
                return false;                      
        }
        return true;
    }

    // Ʈ���� ���Ϳ� ������ �� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            PhotonView collision_PV = collision.transform.GetComponent<PlayerScript>().PV;
            /*�浹�� ĳ������ ����䰡 �����̸� �� ĳ���� ��ũ��Ʈ�� ������ ����*/
            if (collision_PV.IsMine)
                collision.transform.GetComponent<PlayerScript>().IsRound2_Trigger = true;

            /*��� Ʈ���� �ȿ� �������� samestart����*/
            if(AllInTrigger())
                photonView.RPC("samestart", RpcTarget.AllViaServer);               
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PhotonView collision_PV = collision.transform.GetComponent<PlayerScript>().PV;
            /*�浹�� ĳ������ ����䰡 �����̸� �� ĳ���� ��ũ��Ʈ�� ������ ����*/
            if (collision_PV.IsMine)
                collision.transform.GetComponent<PlayerScript>().IsRound2_Trigger = false;


        }
    }

    /*�Ȱ��� �����ϱ� ���ؼ�*/
    [PunRPC]
    void samestart()
    {
        bullet.BulletScriptTriiger = true;
    }


    
}
