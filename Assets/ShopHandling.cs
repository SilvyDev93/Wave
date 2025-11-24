using UnityEngine;

public class ShopHandling : MonoBehaviour
{
    [SerializeField] float uiOffset;
    [SerializeField] int weaponCount;

    [Header("References")]
    [SerializeField] Transform inventoryTransform;
    [SerializeField] Transform shopTransform;
    [SerializeField] ShopMoneyDisplay shopMoneyDisplay;

    [Header("Prefabs")]
    [SerializeField] GameObject weaponStore;
    [SerializeField] GameObject weaponInventory;

    void CreateWeaponInventoryArray()
    {
        foreach (Transform child in inventoryTransform)
        {
            Destroy(child.gameObject);
        }
        
        /*
        for (int i = 0; i < weaponCount; i++)
        {
            GameObject uiObject = Instantiate(weaponInventory, inventoryTransform);
            Vector2 pos = inventoryTransform.position;
            pos.y -= i * uiOffset;
            uiObject.transform.position = pos;  
        }
        */

        CreateObjects(weaponInventory, inventoryTransform, weaponCount);
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

    private void OnEnable()
    {
        CreateWeaponInventoryArray();
        shopMoneyDisplay.SetPlayerMoney();       
    }

    private void Start()
    {
        CreateWeaponShopArray();
    }
}
