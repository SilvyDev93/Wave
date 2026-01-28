using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DoorBehavior : MonoBehaviour
{
    [Header("Lid Position")]
    [SerializeField] float targetYUpper; Vector3 originalPositionUpper; Vector3 targetPositionUpper;
    [SerializeField] float targetYLower; Vector3 originalPositionLower; Vector3 targetPositionLower;

    [Header("Configuration")]
    [SerializeField] float lidMoveSpeed;

    [Header("References")]
    [SerializeField] Transform upperLid;
    [SerializeField] Transform lowerLid;

    public void DoorActivation(bool active)
    {
        switch(active)
        {
            case true:
                targetPositionUpper = new Vector3(originalPositionUpper.x , targetYUpper, originalPositionUpper.z);
                targetPositionLower = new Vector3(originalPositionLower.x, targetYLower, originalPositionLower.z);
                break;

            case false:
                targetPositionUpper = originalPositionUpper;
                targetPositionLower = originalPositionLower;
                break;
        }
    }

    void LidMovement()
    {
        float step = lidMoveSpeed * Time.deltaTime;

        upperLid.position = Vector3.MoveTowards(upperLid.position, targetPositionUpper, step);
        lowerLid.position = Vector3.MoveTowards(lowerLid.position, targetPositionLower, step);
    }

    void Update()
    {
        LidMovement();
    }

    void Awake()
    {
        originalPositionUpper = upperLid.position;
        originalPositionLower = lowerLid.position;

        targetPositionUpper = originalPositionUpper;
        targetPositionLower = originalPositionLower;
    }
}
