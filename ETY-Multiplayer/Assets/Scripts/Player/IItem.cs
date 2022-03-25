using System.Collections;
using UnityEngine;
public interface IItem
{
    void PrimaryUse(GameObject obj);
    void SecondaryUse(GameObject obj);
    void Cancel(GameObject obj);
    void Equip(GameObject obj);
    void Unequip(GameObject obj);
    void Drop(InventoryScript invscr, int slot);
    void Pickup(InventoryScript invscr, int slot);
    void Delete(InventoryScript invscr, int slot);
}