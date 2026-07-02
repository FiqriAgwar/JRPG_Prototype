using System.Collections.Generic;
using UnityEngine;

// Credit to NotSlot

public abstract class EditableShape : MonoBehaviour
{
    [SerializeField]
    public bool isClosedShape = true;

    [HideInInspector]
    public List<Vector3> points = new List<Vector3>();

    public void AddPoint(Vector3 position)
    {
        points.Add(position);
    }

    public void InsertPoint(int index, Vector3 position)
    {
        points.Insert(index, position);
    }

    public void RemovePoint(int selected)
    {
        points.RemoveAt(selected);
    }
}
