using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BS.Systems;

namespace BS.CashFlow
{
    public class Objects : ExtendedMonoBehaviour
    {
      
    }
    public class Income
    {
        public int balance;
        public int income;
        public string name;
        public string date;
        public Income(int balance, int income)
        {
            this.balance = balance;
            this.income = income;
            this.name = "Default";
            this.date = "Default";
        }
        public Income(int balance,int income, string name,string date)
        {
            this.balance = balance;
            this.income = income;
            this.name = name;
            this.date = date;
        }
    }
   public static class Utils
    {
        public static void LineConnection(GameObject go1,GameObject go2,Color color)
        {        
            var lr = go1.AddComponent<LineRenderer>();
            lr.startColor = color;
            lr.endColor = color;
            lr.startWidth =10;
            lr.endWidth = 10;

            lr.SetPosition(0, go1.transform.position);
            lr.SetPosition(1, go2.transform.position);
        }
        public static float CountHighest(List<Income> incomeList)
        {
            float biggestBalance = 0;
            foreach(Income income in incomeList)
            {
                biggestBalance = income.balance;
                if(income.balance > biggestBalance)
                {
                    biggestBalance = income.balance;
                }
            }
            //buffer
            biggestBalance = biggestBalance*=1.2f;
            return biggestBalance;
        }
     
    }

    public enum GraphType
    {
        balance = 0,
        income  = 1,
        expanses = 2,
    }
}
