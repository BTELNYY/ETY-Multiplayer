using System;
using System.Collections.Generic;
using UnityEngine;
public interface IInteract
{
    void interact(PlayerScript ps);
    void interactfail(PlayerScript ps);
}
