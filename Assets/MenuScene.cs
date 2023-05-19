using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MenuScene : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button _CreateOrJoinBT,_QuitBT;
    private void Awake()
    {
        _CreateOrJoinBT.onClick.AddListener(CreateOrJoin);
        _QuitBT.onClick.AddListener(Quit);
    }

    private void Quit()
    {
        PhotonNetwork.LeaveLobby();
        Application.Quit();
    }

    private void CreateOrJoin()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    void Start()
    {
        PhotonNetwork.NickName = "Player "+ Random.Range(1000, 10000);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}
