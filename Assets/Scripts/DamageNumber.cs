using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [Header("Spawn Position Offset")]
    [SerializeField] Vector2 xOffset;
    [SerializeField] Vector2 yOffset;
    [SerializeField] Vector2 zOffset;

    [Header("Number Animation")]
    [SerializeField] float floatSpeed;
    [SerializeField] float floatWait;

    [Header("References")]
    [SerializeField] TextMeshProUGUI number;

    public void SetDamageNumber(int damage)
    {
        number.text = damage.ToString();
    }

    public void SetTextColor(Color color)
    {
        number.color = color;
    }
    
    private void Update()
    {
        Vector3 pos = transform.position;
        pos.y += floatSpeed * Time.deltaTime;
        transform.position = pos;
    }

    void Awake()
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(xOffset.x, xOffset.y), transform.position.y + Random.Range(yOffset.x, yOffset.y), transform.position.z + Random.Range(zOffset.x, zOffset.y));
        transform.position = pos;
    }
}
