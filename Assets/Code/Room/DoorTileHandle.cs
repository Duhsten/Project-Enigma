using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnigma.Rooms;
public class DoorTileHandle : MonoBehaviour
{
    public bool isDoorUnlocked;
    public Vector2Int doorPoint;
    public Vector2Int doorDirection;
    public ProjectEnigma.Rooms.Room Room;
    public void Start()
    {
        Debug.Log($"Door Created @ {this.transform.position}");
    }
    public void SetDoorPoint(Vector2Int p)
    {
        doorPoint = p;
    }
    public void SetRoom(ProjectEnigma.Rooms.Room room)
    {
        this.Room = room;
    }
    public void SetDoorDirection(Vector2Int p)
    {
        doorDirection = p;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("CircleCollider2D");
            if (isDoorUnlocked)
            {
                GetComponent<BoxCollider2D>().isTrigger = true;
                Debug.Log("Door is unlocked");
                GameManager.GetInstance().CreateNewRoom(doorPoint, doorDirection, Room);
            }
            else
                Debug.Log("Door is locked");
                isDoorUnlocked = true;
        }


    }

}
