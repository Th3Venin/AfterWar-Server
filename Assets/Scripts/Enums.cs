using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponTypes
{
    AK47,
    G28,
    M4,
    Revolver,
    Armour,
    Ammo,
    Health,
    NoWeapon
}


public enum MatchStage
{
    stopped,
    waitingForPlayers,
    warmup,
    chooseSpawn,
    match,
    matchEnd
}