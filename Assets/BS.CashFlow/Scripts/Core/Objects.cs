using BS.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BS.CashFlow
{
    public class Objects : ExtendedMonoBehaviour
    {

    }
    public class GraphValue
    {
        public int balance;
        public int income;
        public string name;
        public string date;
        public int incomeDifference;
        public int balanceDifference;
        public GraphValue(int balance, int income)
        {
            this.balance = balance;
            this.income = income;
            this.name = "Default";
            this.date = "Default";
        }
        public GraphValue(int balance, int income, string name, string date)
        {
            this.balance = balance;
            this.income = income;
            this.name = name;
            this.date = date;
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
                biggestBalance = income.balance;
                if(income.balance > biggestBalance)
                {
                    biggestBalance = income.balance;
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

    }
    public class GraphValues
    {
        protected List<GraphValue> valuesList;

        public GraphValues(List<GraphValue> valuesList)
        {
            this.valuesList = valuesList;
        }
        public GraphValues()
        {

        }
        public List<GraphValue> GenerateDummyData(int valueCount)
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
                        difference = valueList[i].balance - valueList[i + 1].balance;
                        if(difference < 0)
                        {
                            difference *= -1;
                        }
                        if(valueList[i].balance > valueList[i + 1].balance)
                        {
                            difference *= -1;
                        }
                        valueList[i + 1].balanceDifference = difference;
                    }
                    if(graphType == GraphType.income)
                    {
                        difference = valueList[i].income - valueList[i + 1].income;
                        if(difference < 0)
                        {
                            difference *= -1;
                        }
                        if(valueList[i].income > valueList[i + 1].income)
                        {
                            difference *= -1;
                        }
                        valueList[i + 1].incomeDifference = difference;
                    }


                }
              
            }



            return valueList;
        }
    }
    public enum GraphType
    {
        balance = 0,
        income = 1,
        outcome = 2,
    }
}
