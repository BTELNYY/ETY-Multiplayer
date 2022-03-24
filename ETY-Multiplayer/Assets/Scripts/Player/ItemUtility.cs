using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemUtility : MonoBehaviour
{
    //gameObjectsToLoad
    static IDictionary<Items, GameObject> ItemList = new Dictionary<Items, GameObject>();
    void Start()
    {
        foreach (Items Item in (Items[])Items.GetValues(typeof(Items)))
        {
            try
            {
                GameObject obj = Resources.Load<GameObject>("Prefabs/Items/" + GetItemID(Item));
                ItemList.Add(Item, obj);
            }
            catch //if loading fails, it will be skipped and replaced with the none item
            {
                GameObject obj = Resources.Load<GameObject>("Prefabs/Items/none");
                ItemList.Add(Item, obj);
            }
        }
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
    public static GameObject GetGameObject(Items item)
    {
        return ItemList[item];
    }
    public static string GetItemName(Items item)
    {
        return item switch
        {
            Items.yandere_knife => "Yandere Knife",
            Items.level_one_key => "Level 1 Key",
            Items.level_two_key => "Level 2 Key",
            Items.level_three_key => "Level 3 Key",
            Items.level_four_key => "Level 4 Key",
            Items.level_five_key => "Level 5 Key",
            Items.admin_gun => "Admin Gun",
            Items.rope => "Rope",
            Items.none => "none",
            _ => "none",
        };
    }
    public static string GetItemID(Items item)
    {
        return item switch
        {
            Items.yandere_knife => "yandere_knife",
            Items.level_one_key => "level_one_key",
            Items.level_two_key => "level_two_key",
            Items.level_three_key => "level_three_key",
            Items.level_four_key => "level_four_key",
            Items.level_five_key => "level_five_key",
            Items.admin_gun => "admin_gun",
            Items.rope => "rope",
            Items.none => "none",
            _ => "none",
        };
    }
    public static Items GetItem(string id)
    {
        return id switch
        {
            "yandere_knife" => Items.yandere_knife,
            "level_one_key" => Items.level_one_key,
            "level_two_key" => Items.level_two_key,
            "level_three_key" => Items.level_three_key,
            "level_four_key" => Items.level_four_key,
            "level_five_key" => Items.level_five_key,
            "admin_gun" => Items.admin_gun,
            "rope" => Items.rope,
            _ => Items.none,
        };
    }
}