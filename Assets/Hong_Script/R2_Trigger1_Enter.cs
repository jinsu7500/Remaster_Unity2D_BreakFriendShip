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
    public int Floor;

    void Update()
    {

    }
    /*��� Ʈ���ſ� ���Դ��� üũ*/
    private bool AllInTrigger(int Floor)
    {
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (Floor == 1)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (!players[i].transform.GetComponent<PlayerScript>().IsRound2_Trigger)
                    return false;
            }
        }
        else if(Floor == 2)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (!players[i].transform.GetComponent<PlayerScript>().IsRound2_Trigger2)
                    return false;
            }
        }
        return true;
    }

    // Ʈ���� ���Ϳ� ������ �� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            PhotonView collision_PV = collision.transform.GetComponent<PlayerScript>().PV;
            /*�浹�� ĳ���� ��ũ��Ʈ�� ������ ����*/

            /*ù��° �� Ʈ�����϶�*/
            if (Floor == 1)
                collision.transform.GetComponent<PlayerScript>().IsRound2_Trigger = true;

            else if (Floor == 2)
                collision.transform.GetComponent<PlayerScript>().IsRound2_Trigger2 = true;

            /*��� Ʈ���� �ȿ� �������� samestart���� ������Ŭ���̾�Ʈ ����*/
            if (AllInTrigger(Floor) && PhotonNetwork.IsMasterClient)
                photonView.RPC("samestart", RpcTarget.AllViaServer);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PhotonView collision_PV = collision.transform.GetComponent<PlayerScript>().PV;
            /*�浹�� ĳ���� ��ũ��Ʈ�� ������ ����*/

            /*ù��° �� Ʈ�����϶�*/
            if (Floor == 1)
                collision.transform.GetComponent<PlayerScript>().IsRound2_Trigger = false;

            else if (Floor == 2)
                collision.transform.GetComponent<PlayerScript>().IsRound2_Trigger2 = false;

            /*��� Ʈ���� �ȿ� �������� samestart���� ������Ŭ���̾�Ʈ ����*/
            if (AllInTrigger(Floor) && PhotonNetwork.IsMasterClient)
                photonView.RPC("samestart", RpcTarget.AllViaServer);
        }
    }

    /*�Ȱ��� �����ϱ� ���ؼ�*/
    [PunRPC]
    void samestart()
    {
        Debug.Log("samestart");
        bullet.BulletScriptTriiger = true;
    }


    
}
