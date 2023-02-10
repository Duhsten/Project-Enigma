using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Room: UnityEngine.Object
{
    
   public string Name { get;  set; }
    public RoomData roomData { get;  set; }
}
