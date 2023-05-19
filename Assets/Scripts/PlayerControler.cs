using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerControler : MonoBehaviourPun
    {
        [SerializeField]
        private Transform _weapon;
        [SerializeField]
        private int _maxHP=3;

        [SerializeField]
        private int _hp;
        public int HP
        {
            get => _hp;
            private set
            {
                if (photonView.IsMine)                 
                    _uIPlayer.SetHPText(_hp = value);
                if (_hp <= 0)
                { 
                    photonView.RPC("Death", RpcTarget.AllViaServer);                   
                }
                
            }
        }
        [SerializeField]
        private PlayerInput _playerInput;
        [SerializeField]
        private InputAction _actionMove;
        [SerializeField]
        private float _speed=5;
        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private UIPlayer _uIPlayer;
        [SerializeField]
        private float _distCamera=10f;
        [SerializeField]
        private bool isPlayable=true;

        private void Awake()
        {
            _hp = _maxHP;
            _playerInput = new PlayerInput();
            _camera = Camera.main;
            _uIPlayer=FindObjectOfType<UIPlayer>();
        }

        private void OnEnable()
        {
           if (photonView.IsMine)
            {
                _playerInput.Player.Enable();
                _playerInput.Player.Fire.performed += Fire_performed;
                _actionMove = _playerInput.Player.Move;
            }
        }

        private void Update()
        {
            if (isPlayable)
                if (photonView.IsMine)
                {
                    Move(_actionMove);
                    Look();
                }
        }

        private void Look()
        {
            var input = _playerInput.Player.Look.ReadValue<Vector2>();
            
            if (input != Vector2.zero)
            {
                Ray ray = Camera.main.ScreenPointToRay(input);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var t = hit.point;
                    transform.LookAt(t);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                }
            }
        }

        private void Move(InputAction actionMove)
        {
            var input=actionMove.ReadValue<Vector2>();
            if (input != Vector2.zero)
            {

                var _targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg +transform.eulerAngles.y;
                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
                 transform.position += targetDirection * _speed * Time.deltaTime;
                _camera.transform.position = transform.position+new Vector3(0,_distCamera,0);
            }
        }

        private void Fire_performed(InputAction.CallbackContext obj)
        {
            if(isPlayable)
                photonView.RPC("Fire", RpcTarget.AllViaServer, _weapon.transform.position, transform.rotation);
        }

        [PunRPC]
        public void Fire(Vector3 pos, Quaternion rot)
        {
            PoolBullet.instance.Fire(pos, rot);
        }

        [PunRPC]
        private void Death()
        {
            isPlayable = false;
            _uIPlayer.SetWonOrLost(photonView.IsMine);
            StartCoroutine(LeaveGame());
        }

        private IEnumerator LeaveGame()
        {
            yield return new WaitForSeconds(10f);
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("MenuScene");
        }

        private void OnCollisionEnter(Collision collision)
        {
            switch(collision.gameObject.tag)
            {
                case "DeathWall":
                    HP -= int.MaxValue;
                    break;
                case "Bullet":
                    HP -= 1;
                    break;
            }
            Debug.Log("OnCollisionEnter Player");
        }
    }
}
