using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDie : MonoBehaviourPun
{
    public Transform[] SpawnPosition;
    public Image youdied;
    public Image someonedied;
    public PhotonView PV;
    

    public string curscene;
    bool turnon = false;
    GameObject LocalPlayer = null;



    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    private void Update()
    { 

    }
 
    void restart()
    {
        PV.RPC("respawn", RpcTarget.AllViaServer);
        //PhotonNetwork.LoadLevel("LoadingScene");
        // SceneManager.LoadScene(curscene);
    }
    //[PunRPC]
    void respawn()
    {
        if (R_NetWorkManager.round == 1 || R_NetWorkManager.round == 5)
            GameObject.FindGameObjectWithTag("init").transform.GetComponent<init_round1>().init_round();
        else if (R_NetWorkManager.round == 3)
            GameObject.FindGameObjectWithTag("init").transform.GetComponent<init_round3>().init_round();
        else if (R_NetWorkManager.round == 4)
            GameObject.FindGameObjectWithTag("init").transform.GetComponent<init_round4>().init_round();
        else if (R_NetWorkManager.round == 6)
            GameObject.FindGameObjectWithTag("init").transform.GetComponent<init_round6>().init_round();

        turnon = false;
    }
    public Transform SelectSpwanPosition()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            /*�ε����� �´� ���������� ����*/
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
                return SpawnPosition[i];
        }
        return null;
    }

    /*Instantiate�� ������ player�� localplayer ������Ʈ ��ȯ�ϴ� �Լ�*/
    public GameObject LocalPlayerObject()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        for(int i =0; i< players.Length; i++)
        {
            if (players[i].transform.GetComponent<PlayerScript>().PV.IsMine)
                return players[i];         
        }
        return null;
    }

    [PunRPC]
    void hitplayer(int index)
    {
        LocalPlayer = LocalPlayerObject();
        PlayerScript PS = LocalPlayer.transform.GetComponent<PlayerScript>();
        PS.isDie = true;

        /*�������� �÷��̾� ���ͳѹ��� �´� ���� Ƚ�� ���ϱ�*/
        R_NetWorkManager.player_die[index - 1] += 1;
        if (index == PS.PV.OwnerActorNr)
            youdied.gameObject.SetActive(true);

        else
            someonedied.gameObject.SetActive(true);

        Invoke("respawn", 2);

        //if (PhotonNetwork.IsMasterClient && !turnon)
        //{
        //    turnon = true;
        //    Invoke("restart", 2);
        //}
    }

    //DieArea ���Խ�
    void OnTriggerEnter2D(Collider2D collision)
    {
        /*DieArea �浹�� �±װ� �÷��̾��*/
        if (collision.tag == "Player" && !turnon)
        {
            int actornum = collision.transform.GetComponent<PlayerScript>().PV.OwnerActorNr;
            PV.RPC("hitplayer", RpcTarget.All, actornum);
            turnon = true;
        }
    }



}
