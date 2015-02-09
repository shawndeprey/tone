using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Scroll : MonoBehaviour
{
    public float speed = 50f; // 15f

    private Camera sceneCamera;
    private Vector3 distance = new Vector3(0, 0, 0);
    private Vector3 lastCamPos = new Vector3(0, 0, 0);

    void Start()
    {
        sceneCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        lastCamPos = sceneCamera.transform.position;
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            Vector3 camPos = sceneCamera.transform.position;
            if (lastCamPos != camPos)
            {
                distance = camPos - lastCamPos;
                lastCamPos = camPos;

                Vector2 movement = new Vector2(distance.x * speed, distance.y * speed);
                movement *= Time.deltaTime;
                transform.Translate(movement);
            }
        }
    }
}