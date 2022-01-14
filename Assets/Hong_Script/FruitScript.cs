using Photon.Pun;
using UnityEngine;

public class FruitScript : MonoBehaviourPunCallbacks
{
    public GameObject fruit;

    void Start()
    {
        
    }
  
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            PhotonView collision_PV = collision.transform.GetComponent<PlayerScript>().PV;
            /*players������Ʈ�� ����÷��̾� ����*/
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for(int i = 0; i < players.Length; i++)
            {
                /*players������Ʈ�� ����䰡 �浹�� �÷��̾������ ������ ã��*/
                if (players[i].transform.GetComponent<PlayerScript>().PV == collision_PV.IsMine)
                {
                    fruit.transform.SetParent(players[i].transform,true);
                    fruit.transform.localPosition = new Vector2(0.7f, -0.1f);
                    return;
                }
            }
        }

        if(collision.tag == "ghost")
        {
            fruit.transform.SetParent(collision.transform, true);
            fruit.transform.localPosition = new Vector2(0.0f, -5.3f);
        }

    }
    public void appleSound()
    {
        
    }
}
