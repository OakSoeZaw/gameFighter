using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // You will drag your player in here
    public float smoothSpeed = 5f; // How fast the camera catches up
    public Vector3 offset = new Vector3(0f, 0f, -10f); // -10 Z is required so the camera doesn't sit inside the 2D plane
    
    // Check this box in Unity if you want the classic "infinite jumper" camera that never looks down!
    public bool onlyMoveUp = true; 

    // We use LateUpdate for cameras so it moves AFTER the player has finished moving this frame
    void LateUpdate()
    {
        // Safety check just in case the player isn't assigned
        if (target == null) return;

        // 1. Find where the camera SHOULD be
        Vector3 desiredPosition = target.position + offset;

        // 2. If we only want the camera to go up, stop the Y position from going lower than it currently is
        if (onlyMoveUp && desiredPosition.y < transform.position.y)
        {
            desiredPosition.y = transform.position.y;
        }

        // 3. Smoothly glide from our current position to the new position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // 4. Actually move the camera
        transform.position = smoothedPosition;
    }
}
