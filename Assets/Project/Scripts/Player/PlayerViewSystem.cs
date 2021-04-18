using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerViewSystem : NetworkBehaviour
{
    //Глаза персонажа/обзор от персонажа

    [HideInInspector] public Vector2 mousePos;

    [SerializeField] PlayerMovementSystem playerMovement;
    [HideInInspector] Camera _camera;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isLocalPlayer)
        {
            _camera = Camera.main;
        }
    }
    private void OnEnable()
    {
        playerMovement = GetComponent<PlayerMovementSystem>();
    }

    /// <summary>
    /// Начать слежение за игроком
    /// </summary>
    public void StartLook()
    {
        if (isLocalPlayer)
        {
            _camera.GetComponent<CameraMovement>().target = this.transform;
            _camera.GetComponent<Camera>().orthographicSize = 5;
        }
    }

    public void StopLook()
    {
        if (isLocalPlayer)
        {
            _camera.GetComponent<CameraMovement>().target = null;
            _camera.transform.position = new Vector3(0, 8.5f, 10);
            _camera.GetComponent<Camera>().orthographicSize = 20;
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