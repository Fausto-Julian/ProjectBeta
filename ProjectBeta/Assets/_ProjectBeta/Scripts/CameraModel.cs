using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModel : MonoBehaviour
{
    [SerializeField] private float camSpeed;
    [SerializeField] private float borderThickness;
    [SerializeField] private float smoothness;
    [SerializeField] private Transform player;


    private Vector3 offset;
    private Vector3 pos;

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }


    public void CameraMode(Vector2 mousePos, bool mode)
    {
        if (mode == true)
            CameraLockedMovement();
        else
            CameraFreeMovement(mousePos);
    }

    public void CameraLockedMovement()
    {
        pos = player.position + offset;
        transform.position = Vector3.Slerp(transform.position, pos, smoothness);
    }

    public void CameraFreeMovement( Vector2 mousePos )
    {
        pos = transform.position;
        //up
        if (mousePos.y >= Screen.height - borderThickness)
            pos.x -= camSpeed * Time.deltaTime;
        //down
        if (mousePos.y <= borderThickness)
            pos.x += camSpeed * Time.deltaTime;
        //left
        if (mousePos.x <= borderThickness)
            pos.z -= camSpeed * Time.deltaTime;
        //right
        if (mousePos.x >= Screen.height - borderThickness)
            pos.z += camSpeed * Time.deltaTime;

        transform.position = pos;
    }
}
