using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cat : MonoBehaviour
{
    [SerializeField] private float _speed = 200f;

    private Transform _roomba;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _roomba = GameObject.Find("roomba").transform;
    }

    void Update()
    {
        if (_roomba != null) {
            _rb.linearVelocity = new Vector3(
                (Mathf.Clamp(_roomba.position.x - transform.position.x, -1f, 1f))*_speed*Time.deltaTime, 
                _rb.linearVelocity.y, 
                (Mathf.Clamp(_roomba.position.z - transform.position.z, -1f, 1f))*_speed*Time.deltaTime
            );

            Vector3 dir = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = rotation;
            _speed += Time.deltaTime;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("wall") || other.gameObject.CompareTag("Door")) {
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
