using UnityEngine;

public class RTSCamera : Photon.PunBehaviour {
    [SerializeField]
    private GameObject world;

    private const int multTimeSpeed = 20;

    private int ScrollActiveWidth = 120;
    private float currentXScrollSpeed;
    private float currentZScrollSpeed;
    private float maxScrollSpeed = 10;


    private float LeftLevelArea;
    private float RightLevelArea;
    private float TopLevelArea;
    private float BottomLevelArea;
    //private float margin;


    private const float MinHeight = 20;
    private const float MaxHeight = 300;

    private const int MaxZoomSpeed = 1000;
    private int zoomAccel = 500;
    private int zoomDecAccel = 10;
    private float zoomSpeed = 0;

    private bool isFollowing = false;

    // Use this for initialization
    void Start() {
        OnStartFollowing();
        
        Vector3 dimensions = new Vector3(1000, 1, 1000);

        LeftLevelArea = -dimensions.x;
        RightLevelArea = dimensions.x;
        TopLevelArea = dimensions.z;
        BottomLevelArea = -dimensions.z;
        //margin = dimensions.x * 5 / 2;
        //margin = 0;

        GameObject mask_L = Instantiate(Resources.Load<GameObject>("UI/UIMask"));
        mask_L.GetComponent<RectTransform>().sizeDelta = new Vector2(ScrollActiveWidth, Screen.height);
        GameObject mask_R = Instantiate(Resources.Load<GameObject>("UI/UIMask"));
        mask_R.GetComponent<RectTransform>().sizeDelta = new Vector2(ScrollActiveWidth, Screen.height);
        GameObject mask_U = Instantiate(Resources.Load<GameObject>("UI/UIMask"));
        mask_U.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width - ScrollActiveWidth, ScrollActiveWidth);
        GameObject mask_D = Instantiate(Resources.Load<GameObject>("UI/UIMask"));
        mask_D.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width - ScrollActiveWidth, ScrollActiveWidth);

    }

    // Update is called once per frame
    void Update() {
        if (isFollowing) {
            MoveCamera();
        }

        
    }

    private void DrawEdge() {
        
    }

    public void OnStartFollowing() {
        //camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        isFollowing = true;
    }


    private void MoveCamera() {
        
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;

        
        Vector3 translation = new Vector3(0, 0, 0);
        float HeightFromGround = transform.position.y;

        // horizontal camera movement
        if (xpos >= 0 && xpos < ScrollActiveWidth && ypos >= 0 && ypos <= Screen.height) {
            // currentXScrollSpeed = -Mathf.Clamp((HeightFromGround - MinHeight) / (MaxHeight - MinHeight) * maxScrollSpeed, maxScrollSpeed / 5, maxScrollSpeed);
            translation.x = -Mathf.Clamp((HeightFromGround - MinHeight) / (MaxHeight - MinHeight) * maxScrollSpeed, maxScrollSpeed / 5, maxScrollSpeed);

        }
        else if (xpos <= Screen.width && xpos > Screen.width - ScrollActiveWidth && ypos >= 0 && ypos <= Screen.height) {
            // currentXScrollSpeed = Mathf.Clamp((HeightFromGround - MinHeight) / (MaxHeight - MinHeight) * maxScrollSpeed, maxScrollSpeed / 5, maxScrollSpeed);
            translation.x = Mathf.Clamp((HeightFromGround - MinHeight) / (MaxHeight - MinHeight) * maxScrollSpeed, maxScrollSpeed / 5, maxScrollSpeed);

        }

        // vertical camera movement
        if (ypos >= 0 && ypos < ScrollActiveWidth && xpos >= 0 && xpos <= Screen.width) {
            // currentZScrollSpeed = -Mathf.Clamp((HeightFromGround - MinHeight) / (MaxHeight - MinHeight) * maxScrollSpeed, maxScrollSpeed / 5, maxScrollSpeed);
            translation.z = -Mathf.Clamp((HeightFromGround - MinHeight) / (MaxHeight - MinHeight) * maxScrollSpeed, maxScrollSpeed / 5, maxScrollSpeed);
        }
        else if (ypos <= Screen.height && ypos > Screen.height - ScrollActiveWidth && xpos >= 0 && xpos <= Screen.width) {
            // currentZScrollSpeed = Mathf.Clamp((HeightFromGround - MinHeight) / (MaxHeight - MinHeight) * maxScrollSpeed, maxScrollSpeed / 5, maxScrollSpeed);
            translation.z = Mathf.Clamp((HeightFromGround - MinHeight) / (MaxHeight - MinHeight) * maxScrollSpeed, maxScrollSpeed / 5, maxScrollSpeed);
        }


        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        Plane world = new Plane(new Vector3(0, 1, 0), new Vector3(0, 0, 0));

        float distance;
        world.Raycast(ray, out distance);
        Vector3 mouseToWorldPosition = ray.GetPoint(distance);

        Vector3 zoom_translation = new Vector3(0, 0, 0);
        zoomSpeed += zoomAccel * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
         
        if (zoomSpeed > 0.1 && transform.position.y > MinHeight) {
            zoomSpeed = Mathf.Min(zoomSpeed, MaxZoomSpeed);
            zoom_translation += (mouseToWorldPosition - transform.position).normalized * zoomSpeed;
            zoomSpeed -= zoomDecAccel * Time.deltaTime;
        }
        else if (zoomSpeed < -0.1 && transform.position.y < MaxHeight) {
            zoomSpeed = Mathf.Max(zoomSpeed, -MaxZoomSpeed);
            zoom_translation += transform.forward * zoomSpeed;
            zoomSpeed += zoomDecAccel * Time.deltaTime;
        }
        else {
            zoomSpeed = 0;
        }

        
        
        

        /*
        float rad = Vector3.Angle(new Vector3(0, -1, 0), transform.forward) * Mathf.Deg2Rad;
        float zMargin = Mathf.Tan(rad) * transform.position.y;

        if ((desiredPosition.z > TopLevelArea - zMargin && translation.z > 0) || (desiredPosition.z < BottomLevelArea - zMargin && translation.z < 0)) { 
            translation.z = 0;
        }
        */

        transform.position += translation + zoom_translation;

        //limit camera to be inside the map
        if (transform.position.x > RightLevelArea) {
            transform.position = new Vector3(RightLevelArea, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < LeftLevelArea) {
            transform.position = new Vector3(LeftLevelArea, transform.position.y, transform.position.z);
        }

        if (transform.position.z > TopLevelArea) {
            transform.position = new Vector3(transform.position.x, transform.position.y, TopLevelArea);
        }
        else if (transform.position.z < BottomLevelArea) {
            transform.position = new Vector3(transform.position.x, transform.position.y, BottomLevelArea);
        }

        if (transform.position.y >= MaxHeight) {
            transform.position = new Vector3(transform.position.x, MaxHeight, transform.position.z);
        }
        else if (transform.position.y <= MinHeight) {
            transform.position = new Vector3(transform.position.x, MinHeight, transform.position.z);
        }


    }

    public static bool AreCameraKeaboardButtonsPressed() {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
            return true;
        }
        else {
            return false;
        }
    }
}
