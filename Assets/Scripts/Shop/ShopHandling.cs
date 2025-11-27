using UnityEngine;

public class ShopHandling : MonoBehaviour
{
    [Header("UI Configuration")]
    [SerializeField] float uiOffset;

    [Header("References")]
    [SerializeField] Transform inventoryTransform;
    [SerializeField] Transform shopTransform;
    [SerializeField] ShopMoneyDisplay shopMoneyDisplay;

    [Header("Prefabs")]
    [SerializeField] GameObject weaponStore;
    [SerializeField] GameObject weaponInventory;

    WeaponHandler weaponHandler;

    void CreateWeaponInventoryArray()
    {
        foreach (Transform child in inventoryTransform)
        {
            Destroy(child.gameObject);
        }

        InventoryList list = inventoryTransform.GetComponent<InventoryList>();

        list.weapons = weaponHandler.GetAllPlayerWeapons();

        CreateObjects(weaponInventory, inventoryTransform, weaponHandler.transform.childCount);

        list.GiveChildrenWeapons();
    }

    void CreateWeaponShopArray()
    {
        ShopList list = shopTransform.GetComponent<ShopList>();

        list.weapons = Resources.LoadAll("Weapons");

        CreateObjects(weaponStore, shopTransform, list.weapons.Length);

        list.GiveChildrenWeapons();
    }

    void CreateObjects(GameObject go, Transform parent, int lenght)
    {
        for (int i = 0; i < lenght; i++)
        {
            GameObject uiObject = Instantiate(go, parent);
            Vector2 pos = parent.position;
            pos.y -= i * uiOffset;
            uiObject.transform.position = pos;
        }
    }

    public void ShopRefresh()
    {
        CreateWeaponInventoryArray();
        shopMoneyDisplay.SetPlayerMoney();
    }

    private void OnEnable()
    {
        ShopRefresh();      
    }

    private void Awake()
    {
        weaponHandler = GameManager.Instance.weaponHandler;
        CreateWeaponShopArray();
    }
}
