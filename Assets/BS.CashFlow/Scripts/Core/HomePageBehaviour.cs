using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BS.CashFlow
{
    public static class Values
    {
        public static List<GraphValue> incomeList = new List<GraphValue>();

        public static List<GraphValue> GenerateDummyData(int valueCount)
        {
            List<GraphValue> valueList = new List<GraphValue>();

            for(int y = 0; y < valueCount; y++)
            {
                int balance = UnityEngine.Random.Range(0, 1000000);
                int income = UnityEngine.Random.Range(5000, 100000);
                List<string> nameList = new List<string>() { "Vodafone", "Unicorn", "GameDev", "Donate" };
                List<string> dateList = new List<string>() { "30.01.2022", "01.02.2022", "02.02.2022" };
                int randomName = UnityEngine.Random.Range(0, nameList.Count);
                int randomDate = UnityEngine.Random.Range(0, dateList.Count);

                GraphValue gV = new GraphValue(balance, income, nameList[randomName], dateList[randomDate]);
                valueList.Add(gV);
            }
            return valueList;
        }
    }
    public class HomePageBehaviour : MonoBehaviour
    {
        public Buttons buttons = new Buttons();
        [System.Serializable]
        public struct Buttons
        {
            public Button Generate;
        }
        public Prefabs prefabs = new Prefabs();
        [System.Serializable]
        public struct Prefabs
        {
            public GameObject dictionaryElement;
        }
        public Rects rects = new Rects();
        [System.Serializable]
        public struct Rects
        {
            public RectTransform contentParent;
        }
        // Start is called before the first frame update
        GraphValues gV;
        void Start()
        {

            CreateValues();
            PopulateDashboard();
            buttons.Generate.onClick.AddListener(delegate
            {
                CreateValues();
                PopulateDashboard();
            });

        }


        void CreateValues()
        {
            if(Values.incomeList.Count > 0)
            {
                Values.incomeList.Clear();
            }

            Values.incomeList = Values.GenerateDummyData(Random.Range(1, 12));
            gV = new GraphValues(Values.incomeList);
            gV.CountDifferences(Values.incomeList, 0);
            gV.CountDifferences(Values.incomeList, (GraphType)1);
            gV.UpdateDisctionaries();

        }
        void PopulateDashboard()
        {
            if(rects.contentParent.childCount > 0)
            {
                DestroyDashboard();
            }
            for(int i = 0; i < gV.dashBoardList.Count; i++)
            {
                var newValue = Instantiate(prefabs.dictionaryElement, rects.contentParent);
                newValue.SetActive(true);
                newValue.transform.SetSiblingIndex(0);
                newValue.GetComponent<DictionaryElementBehaviour>().key.text = Utils.GetStringKeyFromDictionary(gV.dashBoardList[i]).ToString();
                newValue.GetComponent<DictionaryElementBehaviour>().value.text = Utils.GetIntValueFromDictionary(gV.dashBoardList[i]).ToString();
            }

          

         

        }
        void DestroyDashboard()
        {
            foreach(Transform child in rects.contentParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}