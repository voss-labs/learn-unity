using UnityEngine;

[RequireComponent(typeof(Camera))]
public class WideView : MonoBehaviour
{
    Camera view;
    float landscapeFieldOfView;

    void Awake()
    {
        view = GetComponent<Camera>();
        landscapeFieldOfView = view.fieldOfView;
    }

    void Update()
    {
        view.fieldOfView = view.aspect >= 1f
            ? landscapeFieldOfView
            : Camera.HorizontalToVerticalFieldOfView(landscapeFieldOfView, view.aspect);
    }
}
