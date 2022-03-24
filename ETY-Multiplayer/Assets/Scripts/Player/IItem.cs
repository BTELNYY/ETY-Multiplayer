using System.Collections;
using UnityEngine;
public interface IItem
{
    void PrimaryUse(GameObject obj);
    void SecondaryUse(GameObject obj);
    void Cancel(GameObject obj);
    void Drop(InventoryScript invscr);
    void Pickup(InventoryScript invscr);
    void Delete(InventoryScript invscr);
}