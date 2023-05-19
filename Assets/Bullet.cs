
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private float _lifeTime = 0;
    private Rigidbody _rb;
    public float LifeTime { get => _lifeTime; private set => _lifeTime = value; }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        gameObject.SetActive(false);
    }
    public void setLifeTime(float time) 
    {
        gameObject.SetActive(true);
        LifeTime = time;
        StartCoroutine(Cor_LifeTime());
    }

    private IEnumerator Cor_LifeTime()
    {
        while (LifeTime>0)
        {
            yield return new WaitForEndOfFrame();
            Vector3 targetDirection = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f) * Vector3.forward;
            transform.position += targetDirection * _speed * Time.deltaTime;
            LifeTime -= Time.deltaTime;
        }
        this.gameObject.SetActive(false);
    }

}
