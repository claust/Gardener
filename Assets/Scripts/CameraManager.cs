using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    float Speed = 5f;
    [SerializeField]
    float MaxSpeed = 5f;
    [SerializeField]
    Vector2 XRange = new Vector2(- 10, Globals.WorldSize + 10);
    [SerializeField]
    Vector2 ZRange = new Vector2(- 10, Globals.WorldSize);
    [SerializeField]
    float _zoom = 8;

    // Start is called before the first frame update
    void Start()
    {
        var pos = transform.position;
        pos.x = Globals.WorldSize / 2;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        float xAxisValue = Input.GetAxis("Horizontal") * Speed;
        float zAxisValue = Input.GetAxis("Vertical") * Speed;
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        _zoom -= 5 * mouseWheel;
        // Clamp speed
        xAxisValue = Mathf.Clamp(xAxisValue, -MaxSpeed, MaxSpeed);
        zAxisValue = Mathf.Clamp(zAxisValue, -MaxSpeed, MaxSpeed);
        
        // Clamp position
        var x = Mathf.Clamp(transform.position.x + xAxisValue, XRange.x, XRange.y);
        var z = Mathf.Clamp(transform.position.z + zAxisValue, ZRange.x, ZRange.y);
        var y = Mathf.Clamp(_zoom, 1, 10);

        transform.position = new Vector3(x, y, z);
    }
}
