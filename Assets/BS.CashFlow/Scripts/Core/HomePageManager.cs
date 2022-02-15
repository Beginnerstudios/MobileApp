using BS.Systems;
using BS.Systems.UI;
using System.Collections.Generic;
using TMPro;
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
    public class HomePageManager : MonoBehaviour, ILayoutComponent, IUIPageDisplay
    {

        public Assets assets = new Assets();
        [System.Serializable]
        public struct Assets
        {
            public GameObject dictionaryElement;
            public GameObject Generate;
            public TextMeshProUGUI textTitle;
            public RectTransform widgetsHolder;
        }
    
    
    
        GraphValues gV;
        //LayoutComponent
        [field: SerializeField]
        public LayoutComponentProperties Properties { get; set; }



        public void InitLayoutComponent(LayoutComponentType type, bool isActive, RectTransform contentParent, string text, UI sender)
        {
            gameObject.SetActive(isActive);
            Properties = new LayoutComponentProperties(type, gameObject.transform.parent.GetComponent<RectTransform>(), isActive, contentParent, text);
            assets.textTitle.text = Properties.text;
        }
        public void DisplayPage(int displayedValuesCount)
        {
            CreateValues();
            PopulateDashboard();
            CreateGenerateButton();

            void CreateGenerateButton()
            {
                GameObject newBut = Instantiate(assets.Generate, assets.widgetsHolder);
                newBut.SetActive(true);
                newBut.GetComponent<Button>().onClick.AddListener(delegate
                {
                    DisplayPage(0);
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
                if(assets.widgetsHolder.childCount > 0)
                {
                    DestroyWidgets();
                }
                for(int i = 0; i < gV.dashBoardList.Count; i++)
                {
                    var newValue = Instantiate(assets.dictionaryElement, assets.widgetsHolder);
                    newValue.SetActive(true);
                    newValue.transform.SetSiblingIndex(0);
                    newValue.GetComponent<DictionaryElementBehaviour>().key.text = Utils.GetStringKeyFromDictionary(gV.dashBoardList[i]).ToString();
                    newValue.GetComponent<DictionaryElementBehaviour>().value.text = Utils.GetIntValueFromDictionary(gV.dashBoardList[i]).ToString();
                }


            }
        }
        void OnEnable()
        {
            DisplayPage(0);
        } 
        public void DestroyWidgets()
        {
            foreach(Transform child in assets.widgetsHolder)
            {
                Destroy(child.gameObject);
            }
        }
    }
}