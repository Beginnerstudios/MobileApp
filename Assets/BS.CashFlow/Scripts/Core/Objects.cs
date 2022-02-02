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
        public int outcome;
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
        public GraphValue(int outcome)
        {
            this.outcome = outcome;
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

    }
    public class GraphValues
    {
        protected List<GraphValue> valuesList;
       
        public GraphValues(List<GraphValue> valuesList)
        {
            this.valuesList = valuesList;
        }
        public Tuple<int, int> GetDifferences(int index, GraphType graphType)
        {
            int leftDifference = 0;
            int rightDifference = 0;

            if(index + 1 < valuesList.Count )
            {

                if(graphType == GraphType.balance)
                {
                   
                    rightDifference = valuesList[index + 1].balance;
                }
                if(graphType == GraphType.income)
                {
                   
                    rightDifference = valuesList[index + 1].income;
                }

            }
             if(index - 1 >= 0)
            {

                if(graphType == GraphType.balance)
                {
                    leftDifference = valuesList[index - 1].balance;

                }
                if(graphType == GraphType.income)
                {
                    leftDifference = valuesList[index - 1].income;

                }
            
            }
         


            return Tuple.Create(leftDifference, rightDifference);
        }
    }
    public enum GraphType
    {
        balance = 0,
        income = 1,
        outcome = 2,
    }
}
