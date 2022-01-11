using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Cinemachine;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public Animator AN;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NickNameText;



    public bool isGround;
    public bool isDie = false;
    public bool IsRound2_Trigger = false;
    public bool IsRound2_Trigger2 = false;

    Vector3 curPos;
    bool stream_isDie;

    public AudioSource mysfx;
    public AudioClip jumpsfx;


    void Awake()
    {

        // �г��� ǥ��
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;


        //�ڱ� �÷��̾ ����ٴϴ� ī�޶� ����
        //CM�� �ó׸ӽ� ī�޶� ����
        //������ ����
        if (PV.IsMine)
        {
            var CM = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
        }
    }

    void Update()
    {
        /*ĳ���� ������� ����ȭ*/
        
        /*ĳ���� ������ ����ȭ*/
        if (PV.IsMine)
        {
            if (!isDie)
            {
                float axis = Input.GetAxisRaw("Horizontal");
                RB.velocity = new Vector2(4 * axis, RB.velocity.y);

                if (axis != 0)
                {
                    AN.SetBool("isRun", true);
                    PV.RPC("FilpXRPC", RpcTarget.AllBuffered, axis); // �����ӽ� filpX�� ����ȭ���ֱ� ���ؼ� AllBuffered
                }
                else AN.SetBool("isRun", false);
            }

            // ����, �ٴ�üũ
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.5f), 0.07f, 1 << LayerMask.NameToLayer("Ground"));
            AN.SetBool("isJump", !isGround);


            if (Input.GetKeyDown(KeyCode.Space) && isGround && !isDie)
            {
                
                PV.RPC("JumpRPC", RpcTarget.All);              
                JumpSound();
                
            }
            

        }

        //IsMine�� �ƴ� �͵��� �ε巴�� ��ġ ����ȭ
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime*10);
    }


    [PunRPC]
    void FilpXRPC(float axis)
    {
        SR.flipX = axis == -1;
    }// ���� Ű�� ���� ��� True ��ȯ ������ Ű�� ������ ��� False ��ȯ

    [PunRPC]
    void JumpRPC()
    {
        RB.velocity = Vector2.zero;
        RB.AddForce(Vector2.up * 700);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            
        }
    }

    public void JumpSound()
    {
        mysfx.PlayOneShot(jumpsfx);
    }
}