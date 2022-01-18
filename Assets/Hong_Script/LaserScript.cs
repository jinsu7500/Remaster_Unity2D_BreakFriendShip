using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LaserScript : MonoBehaviourPun
{
    private LineRenderer line;
    public GameObject player;
    public GameObject Laser;
    RaycastHit2D hit;
    GameObject LocalPlayer = null;
    public Image youdied;
    public Image someonedied;
    public PhotonView PV;
    public GameObject Elevator;
    public Transform Elevator_Spawn;
    public int hits;
    bool turnon = false;

    bool LaserPoint = false;
    PlayerScript PS;
    public string curscene;
    // Start is called before the first frame update
    private void Start()
    {

    }
    void restart()
    {
        //PhotonNetwork.LoadLevel("LoadingScene");
        PV.RPC("respawn", RpcTarget.AllViaServer);
    }

    /*������ �Ǵ� ���� �������� �¾� �״� ���� ���ֱ� ���ؼ� 0.5�� ������ �� ����*/
    void notDemage() => turnon = false;


    [PunRPC]
    void respawn()
    {
        Elevator.transform.position = new Vector3(Elevator_Spawn.position.x, Elevator_Spawn.position.y, Elevator_Spawn.position.z);
        GameObject.FindGameObjectWithTag("init").transform.GetComponent<init_round1>().init_round();
        Invoke("notDemage", 0.5f);
    }

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
    }

    [PunRPC]
    void lazerhit(int index)
    {
        if (youdied.gameObject.activeSelf == true || someonedied.gameObject.activeSelf == true)
            return;
        turnon = true;
        LocalPlayer = player.transform.GetComponent<round5_test>().character;
        PS = LocalPlayer.transform.GetComponent<PlayerScript>();
        PS.isDie = true;


        if (PS.PV.OwnerActorNr == index)
        {
            youdied.gameObject.SetActive(true);
            R_NetWorkManager.player_die[index - 1] += 1;
        }

        else
            someonedied.gameObject.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("restart", 2);
        }
    }

    PhotonView collider_pv;
    // Update is called once per frame
    void Update()
    {
        firstLaser();
        if (hit.collider != null)
        {
            if(hit.collider.tag == "Player")
                collider_pv = hit.collider.transform.GetComponent<PlayerScript>().PV;
            /*turnon ������ rpc�ѹ� ȣ���Ϸ���*/
            if (hit.collider.tag == "Player" && PV.IsMine && !turnon)
            {
                int actornum = hit.collider.transform.GetComponent<PlayerScript>().PV.OwnerActorNr;
                PV.RPC("lazerhit", RpcTarget.All, actornum);
                
                return;
            }
            LaserPoints();
        }
        else
        {

            line.SetPosition(1, transform.position + new Vector3(10, 0, 0));
        }
    }

    public void firstLaser()
    {
        Debug.DrawRay(transform.position, new Vector3(1, 0, 0) * 50f, new Color(1, 1, 0));
        hit = Physics2D.Raycast(transform.position, new Vector3(1, 0, 0), 50f, 1 << LayerMask.NameToLayer("Defense") | 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Player"));



    }

    public void LaserPoints()
    {
        line.SetPosition(1, hit.point);
    }


}
