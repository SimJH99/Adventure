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
        //카메라의 높이 넓이 구하기
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    //카메라 이동 영역 표시
    private void OnDrawGizmos()
    {
        //맵의 크기 Scene창에 시각적으로 표현
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize);
    }

    void FixedUpdate()
    {
        LimitCameraArea();
    }

    void LimitCameraArea()
    {
        //선형보간
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

        //카메라 제한 영역 구하기
        float lx = mapSize.x * 0.5f - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = mapSize.y * 0.5f - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY, -10);
    }
}
