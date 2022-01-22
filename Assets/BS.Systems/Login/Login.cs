using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BS.Systems

{
    public class Login : ExtendedMonoBehaviour, ISystemComponent
    {
        public Button loginButton;
     
  
        public void Awake()
        {
            AddISystemComponent(this);
        }
        public void OnDestroy()
        {
          RemoveISystemComponent(this);
        }

        private void Start()
        {
            loginButton.onClick.AddListener(delegate{            
                if(Auth.LoggedIn())
                {
                   SceneManager.LoadScene(Scenes.Main.ToString());                
                }
            });
                
        }
        static class Auth
        {           
            public static bool LoggedIn()
            {               
                return true;
            }
        }
       


    }
}