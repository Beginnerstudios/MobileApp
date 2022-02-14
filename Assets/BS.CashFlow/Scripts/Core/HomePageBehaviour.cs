using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BS.Systems.UI;

namespace BS.CashFlow
{
    public static class Values
    {
        public static List<GraphValue> incomeList = new List<GraphValue>();
    }
    public class HomePageBehaviour : MonoBehaviour
    {
        public Buttons buttons = new Buttons();
        [System.Serializable]
        public struct Buttons
        {
            public Button Generate;
        }
        // Start is called before the first frame update
        void Start()
        { 
            GenerateNewValues();
            buttons.Generate.onClick.AddListener(delegate
            {
                GenerateNewValues();
            });
        }

        void GenerateNewValues()
        {
            CreateValues();
        }
        void CreateValues()
        {
            if(Values.incomeList.Count > 0)
            {
                Values.incomeList.Clear();
            }
            GraphValues gV = new GraphValues();
            Values.incomeList = gV.GenerateDummyData(Random.Range(1, 12));
            gV.CountDifferences(Values.incomeList, 0);
            gV.CountDifferences(Values.incomeList, (GraphType)1);         
        }
    
}
}