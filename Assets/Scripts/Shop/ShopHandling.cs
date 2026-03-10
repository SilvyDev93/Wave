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
        for (int i = inventoryTransform.childCount - 1; 0 <= i; i--)
        {
            //Destroy(inventoryTransform.GetChild(i).gameObject);
            DestroyImmediate(inventoryTransform.GetChild(i).gameObject);
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
        if (weaponHandler == null) 
        {
            try
            {
                weaponHandler = GameManager.Instance.weaponHandler;
            }
            catch { }
        }

        CreateWeaponInventoryArray();
        shopMoneyDisplay.SetPlayerMoney();
    }

    private void OnEnable()
    {
        ShopRefresh();
    }

    private void Start()
    {        
        CreateWeaponShopArray();
    }
}
