using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito
{
    public class RandomItem : MonoBehaviour
    {
        public string GetRandomPick()
        {
            
            var dropItem = new Rito.WeightedRandomPicker<string>();

            dropItem.Add
                (
                    ("null", 1800),
                    ("강화석A", 45),
                    ("강화석B", 45),
                    ("강화석C", 45),
                    ("강화석D", 35),

                    ("확률 상승 주문서 10%", 2),
                    ("확률 상승 주문서 5%", 4),
                    ("확률 상승 주문서 3%", 6),

                    ("+1 강화권(0 ~ 5단계)", 4),
                    ("+1 강화권(5 ~ 10단계)", 2),
                    ("+1 강화권(10 ~ 15단계)", 1),

                    ("파괴 보호 주문서 30%", 10),
                    ("파괴 보호 주문서 100%", 1)
            );
            string pick = dropItem.GetRandomPick();

            return pick;
        }
    }
}
