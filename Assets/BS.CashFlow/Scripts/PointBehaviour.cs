using BS.Systems;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace BS.CashFlow
{
    public class PointBehaviour : ExtendedMonoBehaviour
    {

        public Income incomeObj;
        public List<Income> incomeList;
        public TextMeshProUGUI widgetText;
        public Button point;
        public DetailBehaviour detal;
        public int index;
        public GraphType graphType;
        private void Start()
        {
            Init();
        }
        void Init()
        {
            point.onClick.AddListener(delegate
            {
             detal.Refresh(incomeObj);
            });
        


            SetText();
            SetColor();

            void SetText()
            {
                if(graphType == GraphType.balance)
                {
                    widgetText.text = incomeObj.balance.ToString();

                }
                else if(graphType == GraphType.income)
                {
                    widgetText.text = incomeObj.income.ToString();
                }

            }
            void SetColor()
            {
                if(graphType == GraphType.balance)
                {                  
                        widgetText.color = Color.green;                                 
                }
                else if(graphType == GraphType.income)
                {
                    widgetText.color = Color.yellow;
                }
            }

        }

    }
}

