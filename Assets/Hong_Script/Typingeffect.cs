using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Typingeffect : MonoBehaviour
{

    public Text tx;
    public GameObject Ghost;
    public PhotonView PV;
    public Image youdied;
    public Image someonedied;
    GameObject LocalPlayer = null;
    float[] random = new float[13];
    PlayerScript PS;


    private string m_text = "����ȭ ���� �Ǿ����ϴ�";
    // Start is called before the first frame update

    private void Start()
    {
        
        /*���� �̸�*/
        if (PhotonNetwork.IsMasterClient)
        {
            makerandom();
            /*������ ������ �����ѹ� RPC�� ����*/
            PV.RPC("SynRandom", RpcTarget.AllViaServer, random);
        }
        LocalPlayer = LocalPlayerObject();
        PS = LocalPlayer.transform.GetComponent<PlayerScript>();

    }
    void Update()
    {
        if (tx.text == "����ȭ ���� �Ǿ����ϴ�")
        {                   
            if (!PS.isGround || PS.isGround)
                PV.RPC("diefunc", RpcTarget.AllViaServer, PS.PV);
        }
    }
    /*���� �ѹ� ���� �Լ�*/
    void makerandom()
    {
        for (int i = 0; i <= m_text.Length; i++)
        {
            float num = Random.Range(0.1f, 0.6f);
            random[i] = num;
        }
    }

    /*Instantiate�� ������ player�� localplayer ������Ʈ ��ȯ�ϴ� �Լ�*/
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


    IEnumerator countTime(float[] random)
    {
        yield return new WaitForSeconds(2f);
        
        for (int i = 0; i<= m_text.Length; i++)
        {
            if(i == 0)
                Ghost.transform.rotation = Quaternion.Euler(0, 180, 0);
            //Ghost.transform.rotation = Quaternion.Euler(0, 15 * i, 0);

            tx.text = m_text.Substring(0, i);
            if(i== m_text.Length)
            {               
                LocalPlayer = LocalPlayerObject();
                PlayerScript PS = LocalPlayer.transform.GetComponent<PlayerScript>();
                if (!PS.isGround && PS.isGround)
                    PV.RPC("diefunc", RpcTarget.AllViaServer, PS.PV);

                if (PhotonNetwork.IsMasterClient)
                {
                    Ghost.transform.rotation = Quaternion.Euler(0, 0, 0);
                    makerandom();
                    PV.RPC("SynRandom", RpcTarget.AllViaServer, random);
                }
            }
            yield return new WaitForSeconds(random[i]);  
        }
    }
    [PunRPC]
    void diefunc(PhotonView PV)
    {
        if (PV.IsMine)
            youdied.gameObject.SetActive(true);
        else
            someonedied.gameObject.SetActive(true);
        StopCoroutine(countTime(random));
        tx.text = "";

    }


    /*������ ������ �����ѹ� �� Ŭ���̾�Ʈ ������ ����*/
    [PunRPC]
    void SynRandom(float[] num)
    {
        for(int i = 0; i < 13; i++)
        {
            random[i] = num[i];
        }
        /*RPCTARGET.ALLVIA�� ���� startCoroutine ���� ����*/
        StartCoroutine(countTime(random));
    }

}
