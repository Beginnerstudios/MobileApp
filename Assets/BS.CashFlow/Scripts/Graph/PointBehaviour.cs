using BS.Systems;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace BS.CashFlow
{
    public class PointBehaviour : ExtendedMonoBehaviour
    {

        public GraphValue incomeObj;
        public List<GraphValue> incomeList;
        public TextMeshProUGUI widgetText;
        public Button point;
        public DetailBehaviour detail;
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
             detail.Refresh(incomeObj);
            });
        


            SetText();
            SetColor();

            void SetText()
            {
                if (incomeObj != null)
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
                else if(graphType == GraphType.outcome)
                {
                    widgetText.color = Color.cyan;
                }
            }

        }

    }
}

