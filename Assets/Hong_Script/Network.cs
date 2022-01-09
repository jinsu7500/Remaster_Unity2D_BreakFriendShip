using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Network : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    public GameObject DisconnectPanel;
    public InputField NicknameInput;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ValueText, PlayersText, ClickUpgradeText, AutoUpgradeText;
    public Button ClickUpgradeBtn, AutoUpgradeBtn;

    float nextTime;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(540, 960, false);
        
    }

    public void Connect()
    {
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text; // ����, ����Ʈ �� ���� ,�ڱ��ڽ�
        PhotonNetwork.ConnectUsingSettings(); // ������ ����
    }

    public override void OnConnectedToMaster() //������ ������ �κ� �������ϸ� �濡 ������ �� ����
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null);
    }

    public override void OnJoinedRoom()
    {
        ShowPanel(RoomPanel);
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity); // 0,0,0 ȸ������
    }

    void ShowPanel(GameObject CurPanel)
    {
        DisconnectPanel.SetActive(false);
        RoomPanel.SetActive(false);
        CurPanel.SetActive(true);
    }
     PlayerScriptH FindPlayer()
    {
        foreach(GameObject Player in GameObject.FindGameObjectsWithTag("play"))
        
            if (Player.GetPhotonView().IsMine) return Player.GetComponent<PlayerScriptH>();
        return null;
        
    }
    public void click()
    {
        PlayerScriptH Player = FindPlayer();

        Player.value += Player.valuePerClick;
        

    }
    public void ClickUpgrade()
    {
        PlayerScriptH Player = FindPlayer();
        if(Player.value >= Player.clickUpgradeCost)
        {
            Player.value -= Player.clickUpgradeCost;
            Player.valuePerClick += Player.clickUpgradeAdd;
            Player.clickUpgradeCost += Player.clickUpgradeAdd*10;
            Player.clickUpgradeAdd += 2;


            ClickUpgradeText.text = "��� : " + Player.clickUpgradeCost + "\n+" + Player.clickUpgradeAdd + "/Ŭ��"; 
        }
    }

    public void autoUpgrade()
    {
        PlayerScriptH Player = FindPlayer();
        if (Player.value >= Player.autoUpgradeCost)
        {
            Player.value -= Player.autoUpgradeCost;
            Player.valuePerSecond+= Player.autoUpgradeAdd;
            Player.autoUpgradeCost += 500;
            Player.autoUpgradeAdd += 2;


            AutoUpgradeText.text = "��� : " + Player.autoUpgradeCost + "\n+" + Player.autoUpgradeAdd + "/��";
        }
    }

    void showPlayers()
    {
        string playersText = "";
        foreach(GameObject Player in GameObject.FindGameObjectsWithTag("play"))
        {
            playersText = Player.GetPhotonView().Owner.NickName + "/" +
                Player.GetComponent<PlayerScriptH>().value.ToString() + "\n";

        }
        PlayersText.text = playersText;
    }

    void EnableUpgrade()
    {
        PlayerScriptH Player = FindPlayer();
        ClickUpgradeBtn.interactable = Player.value >= Player.clickUpgradeCost;
        AutoUpgradeBtn.interactable = Player.value >= Player.autoUpgradeCost;
    }
    void ValuePerSecond()
    {
        PlayerScriptH Player = FindPlayer();
        Player.value += Player.valuePerSecond;
    }
    void Update()
    {
        if (!PhotonNetwork.InRoom) return;
        showPlayers();
        EnableUpgrade();

        if(Time.time > nextTime)
        {
            nextTime = Time.time + 1;
            ValuePerSecond();
        }
    }





}
