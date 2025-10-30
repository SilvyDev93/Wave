using UnityEngine;

public class MousePositionReference : MonoBehaviour
{
    public void SpreadCalculation()
    {

    }

    void Start()
    {
        GameManager.Instance.debugManager.mousePosition = this.gameObject;
    }
}
