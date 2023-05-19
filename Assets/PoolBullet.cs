using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using System.Collections;

public class PoolBullet : MonoBehaviourPunCallbacks
{

    public static PoolBullet instance;
    [SerializeField]
    private float _lifeTimne = 5f;
    [SerializeField]
    private string _bullet;
    [SerializeField]
    private List<Bullet> bullets = new();
    private void Start()
    {
        instance = this;
            for (var i = 0; i < 30; i++)
            {
                var bul = PhotonNetwork.Instantiate(_bullet, Vector3.zero, Quaternion.identity);
                bul.transform.parent = transform;
                bullets.Add(bul.GetComponent<Bullet>());
            }
    }
    public void Fire(Vector3 pos, Quaternion rot)
    {
        Bullet bul;
        if (bullets.Any(t => !t.gameObject.activeSelf))
        {
            bul = bullets.First(t => !t.gameObject.activeSelf);
            bul.setLifeTime(_lifeTimne);
            bul.transform.position = pos;
            bul.transform.rotation = rot;
        }
        else
        {
            bul = PhotonNetwork.Instantiate(_bullet, Vector3.zero, Quaternion.identity).GetComponent<Bullet>();
            bul.transform.parent = transform;
            bul.setLifeTime(_lifeTimne);
            bul.transform.position = pos;
            bul.transform.rotation = rot;
            bullets.Add(bul);
        }
    }
}
