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
    public GameObject player;

    public string curscene;
    bool turnon = false;
    GameObject LocalPlayer = null;
    public GameObject apple;



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

    /*���忡 �´� �ʱ�ȭ�Լ� ȣ��*/
    [PunRPC]
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
        Debug.Log("respawn�Լ�");
        turnon = false;
        apple.SetActive(true);
    }

    /*���� ���� ������ ��ȯ�Լ�*/
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
        turnon = true;
        /*�������� ���ÿ� �������� �� �״� ȭ�� �ѹ��� ��µǵ��� �ϱ� ���ؼ� ���*/
        if (youdied.gameObject.activeSelf == true || someonedied.gameObject.activeSelf == true)
            return;

        /*���� ĳ���� isDle�����ν� �������̰� �ϱ�*/
        LocalPlayer = LocalPlayerObject();
        PlayerScript PS = LocalPlayer.transform.GetComponent<PlayerScript>();
        int num = player.transform.GetComponent<test>().get_player_index(index);
        Debug.Log(num);
        PS.isDie = true;
        R_NetWorkManager.player_die[num] += 1;
        /*�������� �÷��̾� ���ͳѹ��� �´� ���� Ƚ�� ���ϱ�*/
        if (index == PS.PV.OwnerActorNr)
        {
            youdied.gameObject.SetActive(true);        
        }


        else
            someonedied.gameObject.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("restart", 2);
        }

        //if (PhotonNetwork.IsMasterClient && !turnon)
        //{
        //    turnon = true;
        //    Invoke("restart", 2);
        //}
    }


    //DieArea ���Խ�
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" || collision.tag != "ghost")
            return;
        LocalPlayer = LocalPlayerObject();
        PlayerScript PS = LocalPlayer.transform.GetComponent<PlayerScript>();
        PhotonView collision_pv = collision.transform.GetComponent<PlayerScript>().PV;
        
        /*DieArea �浹�� �±װ� �÷��̾� �߽����� RPCȣ��*/
        if (collision.tag == "Player" && collision_pv == PS.PV && !turnon)
        {
            int actornum = collision.transform.GetComponent<PlayerScript>().PV.OwnerActorNr;
            PV.RPC("hitplayer", RpcTarget.All, actornum);          
        }


    }
}
