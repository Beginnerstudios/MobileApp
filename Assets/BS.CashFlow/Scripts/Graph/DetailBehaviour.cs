using BS.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace BS.CashFlow
{
    public class DetailBehaviour : ExtendedMonoBehaviour
    {

        public TextMeshProUGUI balance;
        public TextMeshProUGUI income;
        new public TextMeshProUGUI name;
        public TextMeshProUGUI date;
        public Button generate;
        public GraphsManager sender;
        private void Start()
        {
            AddListeners();
        }
        void AddListeners()
        {

            generate.onClick.AddListener(delegate
            {
                sender.Generate();
                if(sender.incomeList.Count >= 3)
                {
                    sender.buttons.threeMonths.interactable = true;
                }
                if(sender.incomeList.Count >= 6)
                {
                    sender.buttons.sixMonths.interactable = true;
                }
                sender.buttons.all.interactable = true;


            });
        }
        public void Refresh(GraphValue incomeObj)
        {
            balance.text = incomeObj.balance.ToString();
            balance.color = Color.green;
            income.text = incomeObj.income.ToString();
            income.color = Color.yellow;
            name.text = incomeObj.name;
            date.text = incomeObj.date;
        }
        public void RefreshConnection(GraphValue incomeObj, GraphType graphType)
        {
            int value =0;
            if(graphType == GraphType.balance)
            {
                 value = incomeObj.balanceDifference;
            
            }
            if(graphType == GraphType.income)
            {
                 value = incomeObj.incomeDifference;            
            }
            if(value > 0)
            {
                balance.text = "+"+value.ToString();
            }
            else
            {
            balance.text = value.ToString();
            }
            balance.color = Utils.DeterminateColorFromValue(value);
            income.text = "";         
            name.text = "";
            date.text = "";
        }


    }
}

