using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    [SerializeField] private List<BoxCollider2D> colliders = new List<BoxCollider2D>();

    public void Resize(float width)
    {
        foreach (var collider in colliders)
            collider.size = new Vector2(width, collider.size.y);
    }
}
