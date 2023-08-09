using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimReticle : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    public float currentRotationZ = 0;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aim = mousePos - transform.position; // rotation
        float rotZ = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg; // gives angle in radians using aim
        currentRotationZ = rotZ;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
