using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BS.Systems;
using TMPro;
  
namespace BS.CashFlow
{
	public class GraphBehaviour : ExtendedMonoBehaviour
	{

	public TextMeshProUGUI graphTitle;
        public GraphType graphType;
        public Color color;

        public RectTransform points;
        public RectTransform lines;
        public RectTransform labels;
       
        private void Start()
        {
         
            if(graphType == GraphType.balance)
            {
                color= Color.green;
            }
            else if(graphType == GraphType.income)
            {
                color = Color.yellow;
            }
            else
            {
                color = Color.cyan;
            }
            graphTitle.text = graphType.ToString();
            graphTitle.color = color;
        }
    }
}
