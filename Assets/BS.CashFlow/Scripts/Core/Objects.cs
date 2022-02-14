using BS.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BS.CashFlow
{
    public enum GraphObject { point, connection }
    public interface IGraphValueDisplay
    {
        public void DisplayExistingValues(int displayedValuesCount);
    }
    public class Objects : ExtendedMonoBehaviour
    {
    
    }
    public class GraphValue
    {
        public Dictionary<string, int> balanceDict = new Dictionary<string, int>();
        public Dictionary<string, int> incomeDict = new Dictionary<string, int>();
        public Dictionary<string, int> balanceDifferenceDict = new Dictionary<string, int>();
        public Dictionary<string, int> incomeDifferenceDict = new Dictionary<string, int>();
        public Dictionary<string, string> nameDict = new Dictionary<string, string>();
        public Dictionary<string, string> dateDict = new Dictionary<string, string>();
        //dashboard
  


        public GraphValue(int balance, int income, string name, string date)
        {
            CreateDictionaries(balance, income, name, date);     
        }
        void CreateDictionaries(int balance, int income, string name, string date)
        {
            balanceDict = new Dictionary<string, int>();
            balanceDict.Add("Balance", balance);
            incomeDict = new Dictionary<string, int>();
            incomeDict.Add("Income ", income);
            nameDict = new Dictionary<string, string>();
            nameDict.Add("Name   ", name);
            dateDict = new Dictionary<string, string>();
            dateDict.Add("Date   ", date);
        }

    }
    public static class Utils
    {
        public static void LineConnection(GameObject go1, GameObject go2, Color color)
        {
            var lr = go1.AddComponent<LineRenderer>();
            lr.startColor = color;
            lr.endColor = color;
            lr.startWidth = 10;
            lr.endWidth = 10;

            lr.SetPosition(0, go1.transform.position);
            lr.SetPosition(1, go2.transform.position);
        }
        public static float CountHighest(List<GraphValue> incomeList)
        {
            float biggestBalance = 0;
            foreach(GraphValue income in incomeList)
            {
                foreach(KeyValuePair<string, int> ele in income.balanceDict)
                {
                    biggestBalance = ele.Value;
                    if(ele.Value > biggestBalance)
                    {
                        biggestBalance = ele.Value;
                    }
                }

            }
            //buffer
            biggestBalance = biggestBalance *= 1.2f;
            return biggestBalance;
        }
        public static Color DeterminateColorFromValue(int value)
        {
            if(value > 0)
            {
                return Color.green;
            }
            else if(value.Equals(0))
            {
                return Color.yellow;
            }
            else if(value < 0)
            {
                return Color.red;
            }
            else
            {
                return Color.white;
            }
        }
        public static int GetIntValueFromDictionary(Dictionary<string,int> dictionary)
        {
            int value = 0;
            foreach(KeyValuePair<string, int> ele in dictionary)
            {
              
                value= ele.Value;

            }
            return value;
        }
        public static string GetStringKeyFromDictionary(Dictionary<string, int> dictionary)
        {
            string value = "";
            foreach(KeyValuePair<string, int> ele in dictionary)
            {

                value = ele.Key;

            }
            return value;
        }
        public static string GetStringKeyFromDictionary(Dictionary<string, string> dictionary)
        {
            string value = "";
            foreach(KeyValuePair<string, string> ele in dictionary)
            {

                value = ele.Key;

            }
            return value;
        }
        public static string GetStringValueFromDictionary(Dictionary<string, string> dictionary)
        {
            string value = "";
            foreach(KeyValuePair<string, string> ele in dictionary)
            {

                value = ele.Value;

            }
            return value;
        }
    }
    public class GraphValues
    {
         protected List<GraphValue> valuesList;
         Dictionary<string, int> totalItemsCountDict = new Dictionary<string, int>();
         Dictionary<string, int> totalIncomeDict = new Dictionary<string, int>();
         Dictionary<string, int> currentBalanceDict = new Dictionary<string, int>();
         Dictionary<string, int> latestIncome = new Dictionary<string, int>();

        public List<Dictionary<string,int>> dashBoardList = new List<Dictionary<string, int>>();
        public GraphValues(List<GraphValue> valuesList)
        {
            this.valuesList = valuesList;
            CreateDictionaries();
        }
     

        void CreateDictionaries()
        {
            totalItemsCountDict = new Dictionary<string, int>();
            totalIncomeDict = new Dictionary<string, int>();                 
            currentBalanceDict = new Dictionary<string, int>();                 
            latestIncome = new Dictionary<string, int>();                 
        }
        public Tuple<GraphValue, GraphValue> GetSiblings(int index)
        {
            GraphValue leftSibling = null;
            GraphValue rightSibling = null;

            if(index + 1 < valuesList.Count)
            {
                rightSibling = valuesList[index + 1];
            }
            if(index - 1 >= 0)
            {
                leftSibling = valuesList[index - 1];
            }
            return Tuple.Create(leftSibling, rightSibling);
        }
        public List<GraphValue> CountDifferences(List<GraphValue> valueList, GraphType graphType)
        {
             int difference = 0;
                for(int i = 0; i < valueList.Count - 1; i++)
                {
                    if(i + 1 < valueList.Count)
                    {
                        if(graphType == GraphType.balance)
                        {
                            int value = Utils.GetIntValueFromDictionary(valueList[i].balanceDict);
                            int value1 = Utils.GetIntValueFromDictionary(valueList[i + 1].balanceDict);

                            difference = value - value1;

                            if(difference < 0)
                            {
                                difference *= -1;
                            }
                            if(value > value1)
                            {
                                difference *= -1;
                            }

                            valueList[i + 1].balanceDifferenceDict.Add("Balance Difference", difference);


                        }
                        if(graphType == GraphType.income)
                        {
                            int value = Utils.GetIntValueFromDictionary(valueList[i].incomeDict);
                            int value1 = Utils.GetIntValueFromDictionary(valueList[i + 1].incomeDict);

                            difference = value - value1;

                            if(difference < 0)
                            {
                                difference *= -1;
                            }
                            if(value > value1)
                            {
                                difference *= -1;
                            }

                            valueList[i + 1].incomeDifferenceDict.Add("Income Difference", difference);

                        }


                    }

                }

          
            return valueList;

        }
        public void UpdateDisctionaries()
        {
            int totalIncome = 0;
            foreach(GraphValue item in valuesList)
            {
              totalIncome+= Utils.GetIntValueFromDictionary(item.incomeDict);
            }
            totalIncomeDict.Add("Total income: ",totalIncome);
            totalItemsCountDict.Add("Total items: ",valuesList.Count);
            currentBalanceDict.Add("Current Balance: ", Utils.GetIntValueFromDictionary(valuesList[valuesList.Count-1].balanceDict));
            latestIncome.Add("Latest income: ", Utils.GetIntValueFromDictionary(valuesList[valuesList.Count-1].incomeDict));

            dashBoardList.Add(totalItemsCountDict);
            dashBoardList.Add(totalIncomeDict);
            dashBoardList.Add(currentBalanceDict);
            dashBoardList.Add(latestIncome);

        }
    }
    public enum GraphType
    {
        balance = 0,
        income = 1,
        outcome = 2,
    }
}
