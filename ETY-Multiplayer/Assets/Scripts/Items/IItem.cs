using System.Collections;
using UnityEngine;
public interface IItem
{
    void PrimaryUse(PlayerScript ps);
    void SecondaryUse(PlayerScript ps);
    void Cancel(PlayerScript ps);
    void Equip(PlayerScript ps);
    void Unequip(PlayerScript ps);
    void Delete(InventoryScript invscr, int slot);
}