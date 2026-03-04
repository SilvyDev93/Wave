using UnityEngine;

public class PickUpAnimation : MonoBehaviour
{
    [SerializeField] float bounceSpeed = 8;
    [SerializeField] float bounceAmplitude = 0.05f;
    [SerializeField] float rotationSpeed = 90;

    float startingHeight;
    float timeOffset;

    void Bounce()
    {
        float finalHeight = startingHeight + Mathf.Sin(Time.time * bounceSpeed + timeOffset) * bounceAmplitude;
        Vector3 position = transform.localPosition;
        position.y = finalHeight;
        transform.localPosition = position;
    }

    void Spin()
    {
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.y += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation);
    }

    void Update()
    {
        Bounce();
        Spin();
    }

    void Start()
    {
        startingHeight = transform.localPosition.y;
        timeOffset = Random.value * Mathf.PI * 2;
    }
}
