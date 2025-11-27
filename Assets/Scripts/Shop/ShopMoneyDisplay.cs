using TMPro;
using UnityEngine;

public class ShopMoneyDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI money;

    public void SetPlayerMoney()
    {
        try
        {
            money.text = GameManager.Instance.playerManager.GetPlayerMoney().ToString();
        }
        catch { }
    }
}
