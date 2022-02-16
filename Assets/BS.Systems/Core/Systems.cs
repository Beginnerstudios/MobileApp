using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace BS.Systems
{
    public class Systems : ExtendedMonoBehaviour
    {
        private void Start()
        {
        }
    }
    public class ExtendedMonoBehaviour : MonoBehaviour
    {
        //System architecture
        static List<ISystemComponent> systemComponentList { get; set; } = new List<ISystemComponent>();
        public static void AddISystemComponent(ISystemComponent sC)
        {
            systemComponentList.Add(sC);
            //  Debug.Log("ADDED: " + sC.ToString());
        }
        public static T GetISystemComponent<T>() where T : ISystemComponent
        {
            foreach(ISystemComponent sC in systemComponentList)
            {
                if(sC.GetType().Equals(typeof(T)))
                {
                    return (T)sC;
                }
            }
            Debug.LogError(typeof(T) + " - this component not exist in systemComponentList.");
            return default(T);
        }
        public static void RemoveISystemComponent(ISystemComponent rC)
        {
            ISystemComponent componentForRemove = null;
            foreach(ISystemComponent sC in systemComponentList)
            {
                if(sC.GetType().Equals(rC.GetType()))
                {
                    componentForRemove = sC;
                }
            }
            systemComponentList.Remove(componentForRemove);

            // Debug.Log("REMOVED: " +componentForRemove.ToString());
        }
        public IEnumerator ShowMessage(TextMeshProUGUI messageDisplay, string message, float delay)
        {
            messageDisplay.GetComponent<Animator>().enabled = true;
            messageDisplay.text = message;
            Debug.Log(message);
            messageDisplay.enabled = true;
            yield return new WaitForSeconds(delay);
            messageDisplay.enabled = false;
            messageDisplay.GetComponent<Animator>().enabled = false;

        }

    }
    public interface ISystemComponent
    {
        public void Awake();
        public void OnDestroy();
    }
    public enum Scenes
    {
        Login,
        Main,
    }
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
