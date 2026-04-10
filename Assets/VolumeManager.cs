using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoBehaviour
{
    Volume volume;
    DepthOfField dof;

    float dofOriginalStart;

    int blurTarget;
    float blurIncreaseFactor;
    bool blurdecreaseActive;
    bool blurReturnActive;

    public void BlurEffect(int target, float increaseFactor)
    {
        dof.gaussianStart.value = dofOriginalStart;

        blurTarget = target;
        blurIncreaseFactor = increaseFactor;

        blurReturnActive = false;
        blurdecreaseActive = true;
    }

    void BlurDecrease()
    {
        if (blurdecreaseActive)
        {
            if (dof.gaussianStart.value > blurTarget)
            {
                dof.gaussianStart.value -= blurIncreaseFactor * Time.deltaTime;
                Debug.Log("Decrease in progress");
            }
            else
            {
                blurdecreaseActive = false;
                blurReturnActive = true;
                Debug.Log("Decrease finished");
            }
        }
    }

    void BlurReturn()
    {
        if (blurReturnActive)
        {
            if (dof.gaussianStart.value < dofOriginalStart)
            {
                dof.gaussianStart.value += blurIncreaseFactor * Time.deltaTime;
                Debug.Log("Return in progress");
            }
            else
            {
                blurReturnActive = false;
                Debug.Log("Return finished");
            }
        }
    }

    private void Update()
    {
        BlurDecrease();
        BlurReturn();
    }

    private void Awake()
    {
        volume = FindAnyObjectByType<Volume>();
        volume.profile.TryGet<DepthOfField>(out dof);
        dofOriginalStart = dof.gaussianStart.value;
    }
}
