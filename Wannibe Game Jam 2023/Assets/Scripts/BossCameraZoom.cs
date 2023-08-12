using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossCameraZoom : MonoBehaviour
{
    BearBoss bearBoss;
    Player player;

    CinemachineVirtualCamera vCam;

    float minZoom = 9.09f;
    float padding = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        bearBoss = FindObjectOfType<BearBoss>();
        player = FindObjectOfType<Player>();

        vCam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 bearBossPos = bearBoss.transform.position;
        Vector2 playerPos = player.transform.position;

        float zoom = Mathf.Max(Mathf.Abs(playerPos.x - bearBossPos.x) / vCam.m_Lens.Aspect, Mathf.Abs(playerPos.y - bearBossPos.y)) * 0.5f + padding;
        float currentZoom = vCam.m_Lens.OrthographicSize;
        float futureZoom = Mathf.Max(minZoom, zoom);
        vCam.m_Lens.OrthographicSize = Mathf.Lerp(currentZoom, futureZoom, Time.deltaTime);
    }
}
