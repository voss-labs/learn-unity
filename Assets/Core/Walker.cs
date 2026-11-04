using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Walker : MonoBehaviour
{
    const float FallLimit = -20f;
    const float SpawnLabelSeconds = 3f;
    const float RaycastLift = 0.5f;
    const float GroundStick = 2f;
    const int MaxSpawnKeys = 9;

    [SerializeField] Transform view;
    [SerializeField] Transform spawns;
    [SerializeField] float walkSpeed = 2.7f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float lookDegreesPerPixel = 0.12f;
    [SerializeField] float maxDrop = 0.45f;

    CharacterController body;
    float yaw, pitch, verticalSpeed;
    int currentSpawn;
    float spawnLabelUntil;

    public Transform Spawns
    {
        get => spawns;
        set => spawns = value;
    }

    void Start()
    {
        body = GetComponent<CharacterController>();
        GoTo(0);
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        if (keyboard == null || mouse == null)
            return;
        UpdateCursor(keyboard, mouse);
        UpdateSpawnKeys(keyboard);
        if (Cursor.lockState == CursorLockMode.Locked)
            Look(mouse.delta.ReadValue());
        Move(ReadMove(keyboard), keyboard.shiftKey.isPressed ? runSpeed : walkSpeed);
        if (transform.position.y < FallLimit)
            GoTo(currentSpawn);
    }

    void UpdateCursor(Keyboard keyboard, Mouse mouse)
    {
        if (keyboard.escapeKey.wasPressedThisFrame)
            Cursor.lockState = CursorLockMode.None;
        else if (mouse.leftButton.wasPressedThisFrame)
            Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
    }

    void UpdateSpawnKeys(Keyboard keyboard)
    {
        for (var i = 0; i < spawns.childCount && i < MaxSpawnKeys; i++)
            if (keyboard[Key.Digit1 + i].wasPressedThisFrame)
                GoTo(i);
    }

    static Vector2 ReadMove(Keyboard k)
    {
        var x = Axis(k.dKey.isPressed || k.rightArrowKey.isPressed, k.aKey.isPressed || k.leftArrowKey.isPressed);
        var y = Axis(k.wKey.isPressed || k.upArrowKey.isPressed, k.sKey.isPressed || k.downArrowKey.isPressed);
        return Vector2.ClampMagnitude(new Vector2(x, y), 1f);
    }

    static float Axis(bool positive, bool negative) => (positive ? 1f : 0f) - (negative ? 1f : 0f);

    void Look(Vector2 delta)
    {
        yaw += delta.x * lookDegreesPerPixel;
        pitch = Mathf.Clamp(pitch - delta.y * lookDegreesPerPixel, -80f, 80f);
        ApplyLook();
    }

    void ApplyLook()
    {
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        view.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void Move(Vector2 input, float speed)
    {
        var step = transform.TransformDirection(new Vector3(input.x, 0f, input.y)) * (speed * Time.deltaTime);
        if (body.isGrounded)
        {
            if (!HasGround(transform.position + new Vector3(step.x, 0f, 0f)))
                step.x = 0f;
            if (!HasGround(transform.position + new Vector3(step.x, 0f, step.z)))
                step.z = 0f;
            verticalSpeed = -Mathf.Max(GroundStick, speed);
        }
        else
        {
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
        }
        step.y = verticalSpeed * Time.deltaTime;
        body.Move(step);
    }

    bool HasGround(Vector3 feet) =>
        Physics.Raycast(feet + Vector3.up * RaycastLift, Vector3.down, RaycastLift + maxDrop + body.skinWidth);

    public void GoTo(int index)
    {
        currentSpawn = index;
        var spawn = spawns.GetChild(index);
        body.enabled = false;
        transform.position = spawn.position - Vector3.up * view.localPosition.y;
        body.enabled = true;
        verticalSpeed = 0f;
        var look = spawn.Find("Look");
        var direction = look ? (look.position - spawn.position).normalized : spawn.forward;
        yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        pitch = -Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        ApplyLook();
        spawnLabelUntil = Time.time + SpawnLabelSeconds;
    }

    void OnGUI()
    {
        var label = Time.time < spawnLabelUntil ? spawns.GetChild(currentSpawn).name : null;
        Hud.Draw(label, Cursor.lockState != CursorLockMode.Locked);
    }
}
