using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class CameraMng : MonoBehaviour
{
    [SerializeField] Transform camra;
    [SerializeField] Transform target;
    [SerializeField] float speed;

    public Vector2 center;
    public Vector2 mapSize;
    float height;
    float width;

    void Start()
    {
        //ī�޶��� ���� ���� ���ϱ�
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    //ī�޶� �̵� ���� ǥ��
    private void OnDrawGizmos()
    {
        //���� ũ�� Sceneâ�� �ð������� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize);
    }

    void FixedUpdate()
    {
        LimitCameraArea();
    }

    void LimitCameraArea()
    {
        //��������
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

        //ī�޶� ���� ���� ���ϱ�
        float lx = mapSize.x * 0.5f - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = mapSize.y * 0.5f - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY, -10);
    }
}
