using UnityEngine;
using System.Linq;
using Photon.Pun;
public class SpawnPlayer : MonoBehaviourPunCallbacks
{
    public static SpawnPlayer instance;

    private void Awake()
    {
        instance=this;
    }
    void Start()
    {
        var pos = transform.localScale;
        var temp = PhotonNetwork.Instantiate("Player"+PhotonNetwork.CountOfPlayers,
            new Vector3(Random.Range(-pos.x / 2, -pos.x / 2),
                0.5f,
                Random.Range(-pos.y / 2, -pos.y / 2)),
            Quaternion.identity);
        temp.transform.LookAt(transform.position);
    }

    public void StopGame()
    {
        var players=FindObjectsOfType<Player.PlayerControler>();
        foreach (var player in players)
            player.Stop();
    }
}
