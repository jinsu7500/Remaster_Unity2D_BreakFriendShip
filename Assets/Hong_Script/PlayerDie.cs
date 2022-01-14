using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDie : MonoBehaviourPunCallbacks
{
    public Transform[] SpawnPosition;



    public Image youdied;
    public Image someonedied;
    
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

        PhotonNetwork.LoadLevel("LoadingScene");
        // SceneManager.LoadScene(curscene);
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

    //DieArea ���Խ�
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            PhotonView collision_PV = collision.transform.GetComponent<PlayerScript>().PV;
            LocalPlayer = LocalPlayerObject();
            PlayerScript PS = LocalPlayer.transform.GetComponent<PlayerScript>();
            PS.isDie = true;
            R_NetWorkManager.player_die[collision_PV.OwnerActorNr - 1] += 1;
            if (collision_PV.IsMine)
            {
                youdied.gameObject.SetActive(true);
            }
            else
                someonedied.gameObject.SetActive(true);

            if (PhotonNetwork.IsMasterClient && !turnon)
            {
                turnon = true;
                Invoke("restart", 2);
            }

        }
    }



}
