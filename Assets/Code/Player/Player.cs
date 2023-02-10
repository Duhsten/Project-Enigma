using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TeleportPlayer(Vector2 position)
    {
        this.gameObject.transform.transform.position = position;
        Debug.Log($"Teleporting player to {position}");
    }
}
