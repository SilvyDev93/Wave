using UnityEngine;

public class MenuCameraBob : MonoBehaviour
{
    [Header("Camera Bob")]
    [SerializeField] float amount; //0.002f
    [SerializeField] float frequency; // 10 
    [SerializeField] float bobSmooth;

    void CameraBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, bobSmooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * frequency / 2f) * amount * 1.6f, bobSmooth * Time.deltaTime);
        transform.localPosition += pos;
    }

    void Update()
    {
        CameraBob();
    }
}
