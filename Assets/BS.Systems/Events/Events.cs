using System;
using UnityEngine;


namespace BS.Systems.Events
{
    public class Events : ExtendedMonoBehaviour, ISystemComponent
    {
        public event EventHandler OnReferencesLoaded;

        public void Awake()
        {
            AddISystemComponent(this);
        }
        public void OnDestroy()
        {
            RemoveISystemComponent(this);
        }

        public void OnReferencesLoadedMethod()
        {
            OnReferencesLoaded?.Invoke(this, EventArgs.Empty);            
        }
    
    }
    public static class MyEvents
    {
        public static void OnReferencesLoaded()
        {
          //  EventsManager events = UnityEngine.Object.FindObjectOfType<EventsManager>();
         //  events.OnReferencesLoadedMethod();

        }
   
    }
}