using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject shopObject;
    Transform shopPositions;

    GameObject shopLoaded;

    public void SpawnShop()
    {
        shopLoaded = Instantiate(shopObject, shopPositions.GetChild(Random.Range(0, shopPositions.childCount)).transform.position, Quaternion.identity);
    }

    public void DestroyShop()
    {
        Destroy(shopLoaded);
    }

    private void Start()
    { 
        shopPositions = transform.GetChild(0).GetComponent<Transform>();
    }
}
