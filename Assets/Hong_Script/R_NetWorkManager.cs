using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class R_NetWorkManager : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    public InputField NickNameInput;
    public GameObject DisconnectPanel;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public Text WelcomeText;
    public Image Lobby_Img;
    public Text NicknameText;
    public Text LobbyInfoText;
    public InputField RoomInput;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;


    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;
    public GameObject Left;
    public GameObject Masterban;
    public Text baninfoText;
    public Text MasterbaninfoText;
    public Text MastergiveinfoText;
    public GameObject Clientban;
    public GameObject Mastergive;

    [Header("SelectCharacterImagePanel")]
    public GameObject SelectCharacterImagePanel;
    public Image selectImg;

    public PhotonView PV;

    [Header("ETC")]
    public Text StatusText;
    public Image[] img;
    GameObject a = null;
    Player kickplayer = null;
    Player masterplayer = null;
    public GameObject errornotice;
    public GameObject imgnotnotice;
    public GameObject StatusImg;
    public Text StatusTxt;
    public Text RoomTxt;
    public int selectnum = 0;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    //������ ���� , Master������ �����ϸ� OnConnectedToMaster �ݹ�
    public void Connect()
    {
        if (selectnum == 0) imgnotnotice.SetActive(true);
        else PhotonNetwork.ConnectUsingSettings();
    }

    // Master����, ���°� �Ǹ� �κ� ����
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    //JoinLobby() �ݹ��Լ��� ����
    public override void OnJoinedLobby()
    {
        SelectCharacterImagePanel.SetActive(false);
        Debug.Log("�κ�����");
        LobbyPanel.SetActive(true);
        DisconnectPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "�� ȯ���մϴ�.";
        NicknameText.text = PhotonNetwork.LocalPlayer.NickName + " ��";
        Lobby_Img.transform.GetComponent<Image>().sprite = selectImg.GetComponent<Image>().sprite;
        myList.Clear();
    }

    public void CreateRoom()
    {
        if (RoomInput.text != "")
        {
            StatusImg.SetActive(true);
            RoomTxt.text = "�� ������...";
            if (StatusText.text == "JoinedLobby") PhotonNetwork.CreateRoom(RoomInput.text, new RoomOptions { MaxPlayers = 4 });
            else
            {
                errornotice.SetActive(true);
                return;
            }
        }     
    }

    public void OnClickNicknameBtn()
    {
        if (NickNameInput.text != "") SelectCharacterImagePanel.SetActive(true);
        else NickNameInput.GetComponent<Animator>().SetTrigger("On");
    }

    public void MyListClick(int num)
    {
        StatusImg.SetActive(true);
        RoomTxt.text = "�� ������...";
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // �ִ�������
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // ����, ������ư
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // �������� �´� ����Ʈ ����
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (NickNameInput.text != "")
        {
            LobbyPanel.SetActive(false);
            RoomPanel.SetActive(false);
            NickNameInput.text = "";
        }

    }

    // �Լ� createRoom�� ���������� �������� ������ ��� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.JoinLobby();
        errornotice.SetActive(true);
        StatusImg.SetActive(false);
        RoomInput.text = "";
    }

    // �Լ� JoinRandomRoom�� ���������� �������� ������ ��� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomInput.text = "";
    }

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom()
    {
        RoomTxt.text = "�κ� ������...";
        StatusImg.SetActive(true);
        RoomInput.text = "";
        PhotonNetwork.LeaveRoom();

    }
    public void CheckRoomMaster()
    {
        for (int i = 0; i < Left.transform.childCount; i++)
            Left.transform.GetChild(i).transform.GetComponent<P5Script>().CheckroomMaster();
    }

    public void GiveMasterPlayer(Player master)
    {
        MastergiveinfoText.text = master.NickName + "�Կ��� ������ �ֽðڽ��ϱ�?";
        masterplayer = master;
        Mastergive.SetActive(true);
    }

    public void KickPlayer(Player kick)
    {
        MasterbaninfoText.text = kick.NickName + "�� �����Ͻðڽ��ϱ�?";
        kickplayer = kick;
        Masterban.SetActive(true);
        
    }

    public void clickno() => Masterban.SetActive(false);

    public void clickyes()
    {
        PV.RPC("playerout", RpcTarget.All, kickplayer);
        PhotonNetwork.CloseConnection(kickplayer);
        Masterban.SetActive(false);
        kickplayer = null;
    }

    public void MasterGive_clickyes()
    {
        PhotonNetwork.SetMasterClient(masterplayer);
        Mastergive.SetActive(false);
        masterplayer = null;
        
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        CheckRoomMaster();
    }

    public void clickok()
    {
        StatusImg.SetActive(true);
        RoomPanel.SetActive(false);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
    }
    
    void RoomRenewal()
    {
        CheckRoomMaster();
        List<string> namelist = new List<string>();
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
            namelist.Add(PhotonNetwork.PlayerList[i].NickName);
        }
        for (int i = PhotonNetwork.PlayerList.Length; i < 4; i++)
        {
            namelist.Add("");
        }


        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "�� / " + PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ�";

    }
    public override void OnJoinedRoom()
    {
        if (selectnum == 1)   a = PhotonNetwork.Instantiate("P1", Vector3.zero, Quaternion.identity);
        else if (selectnum == 2)  a = PhotonNetwork.Instantiate("P2", Vector3.zero, Quaternion.identity);
        else if (selectnum == 3)  a = PhotonNetwork.Instantiate("P3", Vector3.zero, Quaternion.identity);
        else if (selectnum == 4)  a = PhotonNetwork.Instantiate("P4", Vector3.zero, Quaternion.identity);
        a.transform.localScale = new Vector3(1f, 1f, 1f);
        a.SetActive(true);


        RoomRenewal();

        

        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);

    }

    public void Send()
    {
        string msg = PhotonNetwork.NickName + " : " + ChatInput.text;
        PV.RPC("ChatRPC", RpcTarget.AllBuffered, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    public void inputField_Enter()
    {
        if (Input.GetKeyDown(KeyCode.Return)) Send();
    }

    public void statusImg()
    {
        if(StatusText.text== "Joined" || StatusText.text == "JoinedLobby") StatusImg.SetActive(false);    
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
        inputField_Enter();
        // ��Ʈ��ũ ����ǥ�� ����
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        Debug.Log(StatusText.text);
        statusImg();
        StatusTxt.text = PhotonNetwork.NetworkClientState.ToString(); ;
        //�κ� ���Ӽ� �� �� ���Ӽ� ǥ�� ����
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "�κ� / " + PhotonNetwork.CountOfPlayers + "����";
    }
    private void Awake()
    {
        PhotonNetwork.EnableCloseConnection = true;
        Screen.SetResolution(960, 540, false);

    }



    [PunRPC] // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
    void ChatRPC(string msg)
    {
        bool isInput = false;

        for (int i = 0; i < ChatText.Length; i++)
        {

            if (ChatText[i].text == "")
            {
                Debug.Log("chat if��: " + i);
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        }
        if (!isInput) // ������ ��ĭ�� ���� �ø�
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
    [PunRPC]
    void playerout(Player kickplayer)
    {
        if(PhotonNetwork.LocalPlayer == kickplayer)
        {
            LobbyPanel.SetActive(true);
            baninfoText.text = "������� " + kickplayer.NickName + "���� �����Ͽ����ϴ�.";
            Clientban.SetActive(true);
            
        }
    }
}
