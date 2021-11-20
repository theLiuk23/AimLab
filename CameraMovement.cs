using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public float sens = 500f;
    float xRotation = 0f;

    public Transform playerBody;
    public Camera POV;

    public GameObject menu;
    public GameObject settings;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
		{
            float mouseX = Input.GetAxis("Mouse X") * sens;
            float mouseY = Input.GetAxis("Mouse Y") * sens;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            POV.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up, mouseX);
        }
    }
}