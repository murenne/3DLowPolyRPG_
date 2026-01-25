using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AvatarCustomizationConfig
{
    public static Dictionary<AvatarPartType, int> SavedOutfit = new();

    // 检查是否有存档数据
    public static bool HasData => SavedOutfit.Count > 0;

    // 保存单个部位
    public static void SavePart(AvatarPartType type, int index)
    {
        if (SavedOutfit.ContainsKey(type))
        {
            SavedOutfit[type] = index;
        }
        else
        {
            SavedOutfit.Add(type, index);
        }
    }

    // 获取单个部位 (如果没有存过，默认返回 0)
    public static int GetPartIndex(AvatarPartType type)
    {
        if (SavedOutfit.ContainsKey(type))
        {
            return SavedOutfit[type];
        }
        return 0; // 默认值
    }
}
