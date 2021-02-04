using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float minlimitx;
    public float maxlimitx;
    public float minlimity;
    public float maxlimity;
    public float margin;

    private float _speed = 20;
    private Vector3 _start;
    public GameObject cam;

    private void Start()
    {
        _start = transform.position;
    }

    private void Update()
    {
        Vector3 clampPos;
        clampPos.x = Mathf.Clamp(transform.position.x, minlimitx, maxlimitx);
        clampPos.y = transform.position.y;
        clampPos.z = Mathf.Clamp(transform.position.z, minlimity, maxlimitx);

        transform.position = clampPos;
        if (Input.mousePosition.x > Screen.width - margin) transform.position += transform.right * _speed * Time.deltaTime;
        if (Input.mousePosition.x < 0 + margin) transform.position -= transform.right * _speed * Time.deltaTime;
        if (Input.mousePosition.y > Screen.height - margin) transform.position += transform.up * _speed * Time.deltaTime;
        if (Input.mousePosition.y < 0 + margin) transform.position -= transform.up * _speed * Time.deltaTime;


        if (Input.GetKey(KeyCode.LeftArrow)) transform.position += transform.right * -_speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) transform.position += transform.right * _speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow)) transform.position += transform.up * -_speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow)) transform.position += transform.up * _speed * Time.deltaTime;
    }
}