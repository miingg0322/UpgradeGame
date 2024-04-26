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
                    ("��ȭ��A", 45),
                    ("��ȭ��B", 45),
                    ("��ȭ��C", 45),
                    ("��ȭ��D", 35),

                    ("Ȯ�� ��� �ֹ��� 10%", 2),
                    ("Ȯ�� ��� �ֹ��� 5%", 4),
                    ("Ȯ�� ��� �ֹ��� 3%", 6),

                    ("+1 ��ȭ��(0 ~ 5�ܰ�)", 4),
                    ("+1 ��ȭ��(5 ~ 10�ܰ�)", 2),
                    ("+1 ��ȭ��(10 ~ 15�ܰ�)", 1),

                    ("�ı� ��ȣ �ֹ��� 30%", 10),
                    ("�ı� ��ȣ �ֹ��� 100%", 1)
            );
            string pick = dropItem.GetRandomPick();

            return pick;
        }
    }
}
