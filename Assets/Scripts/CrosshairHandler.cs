using UnityEngine;

public class CrosshairHandler : MonoBehaviour
{
    [SerializeField] Transform crosshair;
    Transform[] pins;
    
    public void SetCrosshairSpread(float spread)
    {
        pins[0].position = new Vector3(transform.position.x, transform.position.y + spread, transform.position.z);
        pins[1].position = new Vector3(transform.position.x, transform.position.y - spread, transform.position.z);
        pins[2].position = new Vector3(transform.position.x - spread, transform.position.y, transform.position.z);
        pins[3].position = new Vector3(transform.position.x + spread, transform.position.y, transform.position.z);
    }

    void GetPins()
    {
        pins = new Transform[4];

        for (int i = 0; i < crosshair.childCount; i++)
        {
            pins[i] = crosshair.GetChild(i);
        }
    }

    public void SetCrosshairActive(bool activeState)
    {
        crosshair.gameObject.SetActive(activeState);
    }

    private void Awake()
    {
        GetPins();
    }
}
