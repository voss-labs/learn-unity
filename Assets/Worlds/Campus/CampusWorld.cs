using UnityEngine;
using UnityEngine.Rendering;

public class CampusWorld : MonoBehaviour
{
    const string NoShadowPrefix = "NoShadow";

    [SerializeField] Walker walker;

    void Awake()
    {
        foreach (Transform solid in Group("Solids"))
            solid.gameObject.AddComponent<BoxCollider>();
        foreach (Transform floor in Group("Floors"))
            floor.gameObject.AddComponent<BoxCollider>().center = new Vector3(0f, -0.5f, 0f);
        foreach (var mesh in Group("Visual").GetComponentsInChildren<MeshRenderer>(true))
            if (mesh.name.StartsWith(NoShadowPrefix))
                mesh.shadowCastingMode = ShadowCastingMode.Off;
        walker.Spawns = Group("Places");
    }

    Transform Group(string groupName)
    {
        foreach (var child in GetComponentsInChildren<Transform>(true))
            if (child.name == groupName)
                return child;
        throw new MissingReferenceException($"campus.glb has no {groupName} group. Export it again from vmaps.");
    }
}
