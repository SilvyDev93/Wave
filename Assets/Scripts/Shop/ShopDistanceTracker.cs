using TMPro;
using UnityEngine;

public class ShopDistanceTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI shopDistance;

    void UpdateDistance()
    {
        int distance = (int) Vector3.Distance(transform.position, Camera.main.transform.position);
        shopDistance.text = "(" + distance.ToString() + "m)";
    }

    private void Update()
    {
        UpdateDistance();
    }
}
