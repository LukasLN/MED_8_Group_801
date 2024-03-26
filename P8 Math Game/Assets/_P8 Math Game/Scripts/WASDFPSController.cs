using UnityEngine;

namespace AstroMath
{
    public class WASDFPSController : MonoBehaviour
    {
        [SerializeField] float moveSpeed;
        [SerializeField] float mouseSensitivity;

        [SerializeField] Transform bodyToRotate;
        float xRotation;

        private void Start()
        {
            //Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            Movement();
            Rotation();
        }

        void Movement()
        {
            float movementX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
            float movementZ = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

            Vector3 movement = new Vector3(movementX, 0, movementZ);
            transform.position += movement;
        }

        void Rotation()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            bodyToRotate.Rotate(Vector3.up * mouseX);
            bodyToRotate.Rotate(Vector3.left * mouseY);
        }
    }
}