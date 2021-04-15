using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerView : NetworkBehaviour
{
    //Глаза персонажа/обзор от персонажа

    [HideInInspector] public Vector2 mousePos;

    [SerializeField] PlayerMovement playerMovement;
    [HideInInspector] Camera _camera;

    public override void OnStartClient()
    {
        base.OnStartClient();

        playerMovement = GetComponent<PlayerMovement>();

        if (isLocalPlayer)
        {
            _camera = Camera.main;
            _camera.GetComponent<CameraMovement>().target = this.transform;
        }
    }

    void Update()
    {
        LookAt();
    }

    //Слежение за мышкой
    void LookAt()
    {
        if (!isLocalPlayer) return;

        /*float posX = Input.GetAxis("Mouse X");
        float posY = Input.GetAxis("Mouse Y");

        Debug.Log($"{posX} - {posY}");*/

        mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - playerMovement.rb2d.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        playerMovement.rb2d.rotation = angle;
    }
}