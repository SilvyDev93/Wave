using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [HideInInspector] public bool shake;

    float strength;
    float initialLowerSpeed;
    float smoothTime;

    public void StartShake(float strength, float initialLowerSpeed, float smoothTime)
    {
        if (!shake)
        {
            this.strength = strength;
            this.initialLowerSpeed = initialLowerSpeed;
            this.smoothTime = smoothTime;

            shake = true;
        }       
    }

    void Update()
    {
        if (shake)
        {
            float randomZ = Random.value - 0.5f;

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, randomZ * strength);
            strength = Mathf.SmoothDamp(strength, 0f, ref initialLowerSpeed, smoothTime);
            strength = Mathf.Clamp(strength, 0f, 50f);

            if (strength <= 0.1f)
            {
                shake = false;
            }
        }
    }
}
