using BS.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;




namespace BS.CashFlow
{
    public class GraphButtonBehaviour : ExtendedMonoBehaviour
    {
        public GraphObject type;
        public GraphValue incomeObj;


        public Button button;
        public TooltipBehaviour tooltip;
        public GraphType graphType;
        public TextMeshProUGUI widgetText;

        private void Start()
        {
            if(type == GraphObject.connection)
            {
                InitConnection();
            }
            if(type == GraphObject.point)
            {
                InitPoint();
            }

        }
        void InitConnection()
        {

            if(graphType == GraphType.balance)
            {
                gameObject.GetComponent<Image>().color = Color.green;
                widgetText.text = Utils.GetIntValueFromDictionary(incomeObj.balanceDifferenceDict).ToString();
                widgetText.color = Color.green;

            }
            if(graphType == GraphType.income)
            {
                gameObject.GetComponent<Image>().color = Color.yellow;
                widgetText.text = Utils.GetIntValueFromDictionary(incomeObj.incomeDifferenceDict).ToString();
                widgetText.color = Color.yellow;

            }
            button.onClick.AddListener(delegate
            {
                tooltip.PopulateConnection(incomeObj, graphType);

            });

        }
        void InitPoint()
        {
            button.onClick.AddListener(delegate
            {
                tooltip.Populate(incomeObj);
            });



            SetText();
            SetColor();

            void SetText()
            {
                if(incomeObj != null)
                {
                    if(graphType == GraphType.balance)
                    {
                        foreach(KeyValuePair<string, int> ele in incomeObj.balanceDict)
                        {
                            widgetText.text = ele.Value.ToString();

                        }


                    }
                    else if(graphType == GraphType.income)
                    {
                        foreach(KeyValuePair<string, int> ele in incomeObj.incomeDict)
                        {
                            widgetText.text = ele.Value.ToString();

                        }
                    }

                }


            }
            void SetColor()
            {
                widgetText.color = Color.cyan;
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

