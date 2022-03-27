using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemUtility : MonoBehaviour, ITick
{
    public GameObject item_none;
    public GameObject level_one_key;
    public GameObject level_two_key;
    public GameObject level_three_key;
    public GameObject level_four_key;
    public GameObject level_five_key;
    public GameObject yandere_knife;
    public GameObject admin_gun;
    public GameObject rope;
    public bool ReloadItemsOnNextTick = false;
    static IDictionary<Items, GameObject> ItemList = new Dictionary<Items, GameObject>();
    void Start()
    {
        Globals.AddITick(this);        
        LoadItems();
    }
    public void Tick()
    {
        if (ReloadItemsOnNextTick)
        {
            ReloadItems();
            ReloadItemsOnNextTick = false;
        }
    }
    void ReloadItems()
    {
        ItemList.Clear();
        LoadItems();
    }
    void LoadItems()
    {
        //yes, I could use a foreach loop, no I am not going to do so
        ItemList.Add(Items.none, item_none);
        ItemList.Add(Items.level_one_key, level_one_key);
        ItemList.Add(Items.level_two_key, level_two_key);
        ItemList.Add(Items.level_three_key, level_three_key);
        ItemList.Add(Items.level_four_key, level_four_key);
        ItemList.Add(Items.level_five_key, level_five_key);
        ItemList.Add(Items.yandere_knife, yandere_knife);
        ItemList.Add(Items.admin_gun, admin_gun);
        ItemList.Add(Items.rope, rope);
    }
    public static GameObject GetItem(Items item)
    {
        return ItemList[item];
    }
    public static ItemBase GetItemScript(Items item)
    {
        return ItemList[item].GetComponent<ItemBase>();
    }
    public enum Items
    {
        none,
        yandere_knife,
        level_one_key,
        level_two_key,
        level_three_key,
        level_four_key,
        level_five_key,
        admin_gun,
        rope,
    }
}