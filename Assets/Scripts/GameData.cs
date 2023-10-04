using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static List<SummonData> summonDatas = new List<SummonData>();
    private static List<AmmoScriptable> selectedAmmos = new List<AmmoScriptable>();
    private static List<int> ammoCount = new List<int>();

    public static void SetAmmoCount(List<int> ammoC)
    {
        ammoCount = ammoC;
    }

    public static List<int> GetAmmoCount()
    {
        return ammoCount;
    }

    public static void SetSelectedAmmos(List<AmmoScriptable> ammo)
    {
        selectedAmmos = ammo;
    }

    public static List<AmmoScriptable> GetSelectedAmmo()
    {
        return selectedAmmos;
    }

    public static List<SummonData> GetSummonDatas()
    {
        return summonDatas;
    }

    public static void SetSummonDatas(List<SummonData> summon)
    {
        summonDatas = summon;
    }
}
