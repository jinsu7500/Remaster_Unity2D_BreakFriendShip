using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class TimerLimit : MonoBehaviourPunCallbacks
{
    public float LimitTime;
    public Text[] ClockText;
    public bool turnon = false;
    public Image youdied;
    bool synon = false;
    public PhotonView PV;
    public test test;

    
    GameObject LocalPlayer = null;
    // Start is called before the first frame update
    void Start()
    {
        // ������ �� �ð� ����ȭ
        PV.RPC("synonfunc", RpcTarget.AllViaServer);
    }
    void restart()
    {
        //PhotonNetwork.LoadLevel("LoadingScene");
        PV.RPC("respawn", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void respawn()
    {
        GameObject.FindGameObjectWithTag("init").transform.GetComponent<init_round3>().init_round();
    }

    /*�ð� ����ȭ ���߱� ���ؼ�*/
    [PunRPC]
    void synonfunc()
    {
        synon = true;
    }

    public GameObject LocalPlayerObject()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].transform.GetComponent<PlayerScript>().PV.IsMine)
                return players[i];
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        /*����ȭ �Ǳ����̸� ����������*/
        if (!synon)
            return;

        /*����ȭ �ǰ� �ð��� 0�̰� turnon����ġ�� false��� ����, turnon ���� ������
         �ѹ��� ����Ǳ� ���ؼ�*/
        if (ClockText[1].text == "0" && !turnon)
        {
            //�׾��� �� ����ϴ� �ڵ�
            LocalPlayer = LocalPlayerObject();
            PlayerScript PS = LocalPlayer.transform.GetComponent<PlayerScript>();
            PS.isDie = true;
            
            youdied.gameObject.SetActive(true);

            if (PhotonNetwork.IsMasterClient && !turnon)
            {             
                Invoke("restart", 2);
            }


            turnon = true;
        }

        /*���� �ð��� 0�ʶ�� ��������, �Ʒ��� ����ǰ� ���� �ʱ����ؼ�, */
        if (ClockText[1].text == "0")
            return;

        /*�ð��� 0�� �ƴ϶�� ��, �� ǥ��*/
        else
        {
            LimitTime -= Time.deltaTime;
            ClockText[0].text = "0" + ((int)(Mathf.Round(LimitTime) / 60)).ToString(); //��
            ClockText[1].text = (Mathf.Round(LimitTime) % 60).ToString(); // ��
        }


    }
}
