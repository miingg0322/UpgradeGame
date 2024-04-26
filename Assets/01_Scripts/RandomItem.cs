using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito
{
    public class RandomItem : MonoBehaviour
    {
        public RanItemData[] ranItemDatas;
        public string GetRandomPick()
        {
            
            var dropItem = new Rito.WeightedRandomPicker<string>();

            foreach (var itemInfo in ranItemDatas[0].itemInfos) // ranItemDatas[]에 Enemy의 level 값(spawnData.level)을 받도록 수정 예정
            {
                dropItem.Add((itemInfo.itemName, itemInfo.weight));
            }

            string pick = dropItem.GetRandomPick();

            return pick;
        }
    }
}
