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


    }
}

