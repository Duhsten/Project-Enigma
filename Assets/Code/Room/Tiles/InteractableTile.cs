
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class InteractableTile : Tile
{
    public bool isDoorUnlocked;
    public CircleCollider2D walkthroughCollider;
    public BoxCollider2D doorCollider;
    public InteractableTile()
    {
       
    }
    
    private void OnEnable()
    {
        
    }
    private void OnDestroy()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void OnValidate()
    {
    }
}
