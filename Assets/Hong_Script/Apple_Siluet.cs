using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Apple_Siluet : MonoBehaviourPunCallbacks
{
    public Image[] IsFruit;
    public Sprite fruit;
    public PhotonView PV;
    public GameObject apple;
    public GameObject apple_silute;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*��� 4�� �� �Ծ����� �����ϴ� �Լ�*/
    public void nextRound()
    {
        for (int i = 0; i < 4; i++)
        {
            if (IsFruit[i].sprite != fruit) return;
        }

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("round2");
    }

    public int playerindex()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            /*�� �÷��̾� �� ���° �÷��̾����� �ε�������*/
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
                return i;
        }
        return -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fruit")
        {
            PV.RPC("isfruit", RpcTarget.AllViaServer, playerindex());
            apple_silute.GetComponent<SpriteRenderer>().sprite = fruit;
            Destroy(apple);
        }

    }

    [PunRPC]
    void isfruit(int index)
    {
        IsFruit[index].sprite = fruit;
        nextRound();
    }


}
