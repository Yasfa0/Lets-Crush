using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static List<SummonData> summonDatas = new List<SummonData>();

    public static List<SummonData> GetSummonDatas()
    {
        return summonDatas;
    }

    public static void SetSummonDatas(List<SummonData> summon)
    {
        summonDatas = summon;
    }
}
