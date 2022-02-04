using BS.Systems;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace BS.CashFlow
{
    public class ConnectionBehaviour : ExtendedMonoBehaviour
    {

        public GraphValue incomeObj;      
        public Button connection;
        public TooltipBehaviour tooltip;
        public GraphType graphType;
       
        private void Start()
        {
            Init();
        }
        void Init()
        {
            connection.onClick.AddListener(delegate
            {             
             tooltip.PopulateConnection(incomeObj,graphType);
            });

        }

    }
}

