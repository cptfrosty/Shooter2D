using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [HideInInspector] public Vector2 mousePos;

    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        LookAt();
    }

    //Слежение за мышкой
    void LookAt()
    {
        /*mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Vector2.Angle(Vector2.right, mousePos - new Vector2(transform.position.x, transform.position.y));
        transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < mousePos.y ? angle : -angle);//немного магии на последок*/

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - playerMovement.rb2d.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        playerMovement.rb2d.rotation = angle;
    }
}