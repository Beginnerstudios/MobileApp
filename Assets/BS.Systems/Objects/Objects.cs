using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace BS.Systems

{
    public class Objects : ExtendedMonoBehaviour
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
    public interface IUIPageDisplay
    {
        public void DisplayPage(int displayedValuesCount);
        public void DestroyWidgets();
    }
    public enum Scenes
    {
        Login,
        Main,
    }


}
