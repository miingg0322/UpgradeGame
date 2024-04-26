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

            foreach (var itemInfo in ranItemDatas[0].itemInfos) // ranItemDatas[]�� Enemy�� level ��(spawnData.level)�� �޵��� ���� ����
            {
                dropItem.Add((itemInfo.itemName, itemInfo.weight));
            }

            string pick = dropItem.GetRandomPick();

            return pick;
        }
    }
}
