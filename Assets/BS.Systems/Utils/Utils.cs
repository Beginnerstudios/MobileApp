using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BS.CashFlow;


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
        public static void CreatePageButton(GameObject gameObject,RectTransform contentParent)
        {
            var but = gameObject.GetComponent<Button>();
            var img = gameObject.GetComponent<Image>();
                           
            but.targetGraphic = img;
            but.onClick.AddListener(delegate
            {
                var butIndex = gameObject.transform.GetSiblingIndex();
                foreach (RectTransform rT in contentParent)
                {
                    if (rT.gameObject.transform.GetSiblingIndex() == butIndex)
                    {                    
                        rT.gameObject.SetActive(true);
                        if (butIndex.Equals(1) || (butIndex.Equals(2)))
                        {
                            rT.gameObject.GetComponent<IGraphValueDisplay>().DisplayExistingValues(0);
                        }
                    }
                    else
                    {
                        rT.gameObject.SetActive(false);
                    }               
                }
            });
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