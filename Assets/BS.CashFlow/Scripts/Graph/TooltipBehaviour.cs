using BS.Systems;
using System.Collections.Generic;
using UnityEngine;


namespace BS.CashFlow
{
    public class TooltipBehaviour : ExtendedMonoBehaviour
    {

        public GameObject DictionaryElement;
        public RectTransform ElementsParent;

        public void Populate(GraphValue incomeObj)
        {
            if(ElementsParent.childCount > 0)
            {
                foreach(Transform child in ElementsParent)
                {
                    Destroy(child.gameObject);
                }
            }
            int elementsCount = 4;
            for(int i = 0; i < elementsCount; i++)
            {
                string key = "";
                string value = "";
                var element = Instantiate(DictionaryElement);
                element.SetActive(true);
                element.transform.SetParent(ElementsParent);        
                var component = element.GetComponent<DictionaryElementBehaviour>();
                if(i == 0)
                {
                    key = Utils.GetStringKeyFromDictionary(incomeObj.balanceDict).ToString();
                    value = Utils.GetIntValueFromDictionary(incomeObj.balanceDict).ToString();
                }
                if(i == 1)
                {
                    key = Utils.GetStringKeyFromDictionary(incomeObj.incomeDict);
                    value = Utils.GetIntValueFromDictionary(incomeObj.incomeDict).ToString();
                }
                if(i == 2)
                {

                    key = Utils.GetStringKeyFromDictionary(incomeObj.nameDict);
                    value = Utils.GetStringValueFromDictionary(incomeObj.nameDict);
                }
                if(i == 3)
                {
                    key = Utils.GetStringKeyFromDictionary(incomeObj.dateDict);
                    value = Utils.GetStringValueFromDictionary(incomeObj.dateDict);

                }
                component.key.text = key;
                component.value.text = value;
            }


        }
        public void PopulateConnection(GraphValue incomeObj, GraphType graphType)
        {

            if(ElementsParent.childCount > 0)
            {
                foreach(Transform child in ElementsParent)
                {
                    Destroy(child.gameObject);
                }
            }
            int elementsCount = 1;
            for(int i = 0; i < elementsCount; i++)
            {
                string key = "";
                string value = "";
                var element = Instantiate(DictionaryElement);
                element.SetActive(true);
                element.transform.SetParent(ElementsParent);
                var component = element.GetComponent<DictionaryElementBehaviour>();
                if(i == 0)
                {
                    if(graphType == GraphType.balance)
                    {
                        key = Utils.GetStringKeyFromDictionary(incomeObj.balanceDifferenceDict).ToString();
                        value = Utils.GetIntValueFromDictionary(incomeObj.balanceDifferenceDict).ToString();
                    }
                    if(graphType == GraphType.income)
                    {
                        key = Utils.GetStringKeyFromDictionary(incomeObj.incomeDifferenceDict).ToString();
                        value = Utils.GetIntValueFromDictionary(incomeObj.incomeDifferenceDict).ToString();
                    }

                }
                
                
                component.key.text = key;
                component.value.text = value;
            }
        }



    }
}

