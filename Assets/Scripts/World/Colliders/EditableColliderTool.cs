using UnityEngine;

// Credit to NotSlot

[RequireComponent (typeof(MeshCollider))]
public sealed class EditableColliderTool : EditableShape
{
    [SerializeField] public float height = 2;

    [SerializeField] public bool reverseTriangles = false;
}
