using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    private Vector3 posCam;

    //Рейкаст
    //Определение попадания

    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (target == null) return;

        posCam = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = posCam;
    }
}
