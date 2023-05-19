using UnityEngine;
using Photon.Pun;
public class SpawnPlayer : MonoBehaviourPunCallbacks
{

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

}
