using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Billboard : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] bool onlyHorizontal;

    [Header("Dynamic Scaling")]
    [SerializeField] bool scalingEnabled;
    [SerializeField] float scaleFactor;

    Vector3 originalScale;

    void BillboardCameraFunc()
    {
        Vector3 distance = transform.position - Camera.main.transform.position;
        
        if (!onlyHorizontal)
        {
            transform.rotation = Quaternion.LookRotation(distance);
        }
        else
        {
            Quaternion rotation = Quaternion.LookRotation(distance);
            Quaternion newRot = new Quaternion(0, rotation.y, 0, rotation.w);
            transform.rotation = newRot;
        }

        if (scalingEnabled)
        {
            float scale = (Camera.main.transform.position - transform.position).magnitude * scaleFactor;

            float scaleX = Mathf.Clamp(scale, originalScale.x, scale);
            float scaleY = Mathf.Clamp(scale, originalScale.y, scale);
            float scaleZ = Mathf.Clamp(scale, originalScale.z, scale);

            Vector3 scaledSize = new Vector3(scaleX, scaleY, scaleZ);
            transform.localScale = scaledSize;
        }        
    }

    void Update()
    {
        BillboardCameraFunc();
    }

    void Start()
    {
        originalScale = transform.localScale;
    }
}
