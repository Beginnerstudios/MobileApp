using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BS.CashFlow;
using System.Collections.Generic;


namespace BS.Systems
{
    public static class Utils
    {      
        public static float[] ValidateLayoutSize(float[] layoutSizes)
        {
            float total = 0;
            foreach(float value in layoutSizes)
            {
                total += value;
            }
            if(total.Equals(1))
            {
                return layoutSizes;
            }
            else
            {
                Debug.Log("Invalid layout size, default was used.");
                return new float[3] { .1f, .8f, .1f };
            }
        }
  
        public static float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if(n < 0)
                n += 360;

            return n;
        }
       
    }
}