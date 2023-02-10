using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform playerTransform; // Reference to the player transform

    public void Start()
    {
        playerTransform = this.gameObject.transform;
    }
    private void Update()
    {
        // Keep the camera's position equal to the player's position
        this.gameObject.transform.GetChild(1).transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z - 10);
    }
}
