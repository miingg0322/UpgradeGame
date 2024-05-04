using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito
{
    public class RandomItem : MonoBehaviour
    {
        public RanItemData[] ranItemDatas;

        private void Start()
        {
            GameManager.Instance.AssignRanItem(this);
        }
        public string[] GetRandomPick()
        {
            
            var dropItem = new Rito.WeightedRandomPicker<string>();

            foreach (var itemInfo in ranItemDatas[GameManager.Instance.DungeonLevel].itemInfos) 
            {
                dropItem.Add((itemInfo.itemName, itemInfo.weight));
            }

            string pick = dropItem.GetRandomPick();
            string grade = ranItemDatas[GameManager.Instance.DungeonLevel].GetGrade(pick);

            string[] info = new string[2];
            info[0] = pick;
            info[1] = grade;

            return info;
        }
    }
}
