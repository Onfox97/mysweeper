using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouse_sensitivity;

    public float zoom_min;
    public float zoom_max;
    public float zoom_speed;

    float zoom_current;

    public bool mouse_inversed = true;

    public Material skybox;

    int boundX;
    int boundY;

    Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();

        zoom_current = cam.orthographicSize;

        boundX =Minefield.field_sizeX;
        boundY = Minefield.field_sizeY;

        zoom_max = (boundX+boundY)/4;

        float poos_newX = boundX/2;
        float poos_newY = boundY/2;

        transform.position = new Vector3(poos_newX,poos_newY,-10);
    }
    void Update()
    {
        Move();
        Zoom();

        float poos_clampedX = Mathf.Clamp(transform.position.x,0,boundX);
        float poos_clampedY = Mathf.Clamp(transform.position.y,0,boundY);

        skybox.SetTextureOffset("_FrontTex",transform.position*SpriteLoaderMaster.spritesConfig.backgroundSpeed);

        float backgroundZoom = zoom_current * SpriteLoaderMaster.spritesConfig.backgroundZoomSpeed;

        Vector2 scale = new Vector2(backgroundZoom*cam.aspect ,backgroundZoom) *SpriteLoaderMaster.spritesConfig.backgroundSize ;

        skybox.SetTextureScale("_FrontTex",scale);

        transform.position = new Vector3(poos_clampedX,poos_clampedY,-10);
    }
    void Move()
    {
        if(Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            
            Vector2 move = new Vector2(mouseX,mouseY) * mouse_sensitivity * Time.deltaTime * (zoom_current/zoom_min);

            if(mouse_inversed) move = -move;

            transform.position += (Vector3)move;
        }
    }
    void Zoom()
    {
        float scroll = -Input.GetAxis("Mouse ScrollWheel");

        zoom_current += scroll *Time.deltaTime * zoom_speed;
        zoom_current = Mathf.Clamp(zoom_current,zoom_min,zoom_max);

        cam.orthographicSize = zoom_current;
    }
}
