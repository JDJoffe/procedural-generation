using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

// basic WASD-style movement control
public class FpsMovement : MonoBehaviour
{
    #region var
    [SerializeField] private Camera headCam;
    public float speed = 6.0f;
    public float gravity = -9.8f;
    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;
    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;
    private float rotationVert = 0;
    private CharacterController charController;
    #endregion
    void Start()
    {
        // get the controller
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // call functions
        MoveCharacter();
        RotateCharacter();
        RotateCamera();
    }
    // character controller movement/rotation & cam rotation
    #region move
    private void MoveCharacter()
    {
        // i am speed
        if (Input.GetKey(KeyCode.LeftShift)){ speed = 9f; }
        else { speed = 6; }
        // input get axises
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        // v3 var = new  x and z values 
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        // clamp the movement to speed so it doesnt change
        movement = Vector3.ClampMagnitude(movement, speed);
        // apply gravity
        movement.y = gravity;
        // multiply movement by time.deltatime so stuff actually moves
        movement *= Time.deltaTime;
        // move the object in worldspace i nthe direction of movement
        movement = transform.TransformDirection(movement);
        // move the character 
        charController.Move(movement);
    }
    #endregion
    #region rotate character
    private void RotateCharacter()
    {
        // rotate the character on the y axis based on the x position of the mouse (character rotates left and right)
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
    }
    #endregion
    #region rotate cam
    private void RotateCamera()
    {
        // float -= the mouses y position * the sensitivity
        rotationVert -= Input.GetAxis("Mouse Y") * sensitivityVert;
        // clamp rotationvert between minvert and maxvert to make it smoother
        rotationVert = Mathf.Clamp(rotationVert, minimumVert, maximumVert);
        // the  local transform euler angles of the camera = a new v3 of rotationvert and the current camera transform euler angles y
        headCam.transform.localEulerAngles = new Vector3(rotationVert, headCam.transform.localEulerAngles.y, 0);
    }
    #endregion
}
