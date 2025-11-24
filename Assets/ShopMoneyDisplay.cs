using TMPro;
using UnityEngine;

public class ShopMoneyDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI money;

    public void SetPlayerMoney()
    {
        try
        {
            money.text = GameManager.Instance.GetPlayerMoney().ToString();
        }
        catch { }
    }
}
