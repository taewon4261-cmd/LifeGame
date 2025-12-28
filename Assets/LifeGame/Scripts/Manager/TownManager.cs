using LifeGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LifeGame.Manager
{
    public class TownManager : MonoBehaviour
    {
        public void GoHome()
        {
            if(SceneNavigator.Instance != null)
                SceneNavigator.Instance.GoHome();
        }

        public void GoToShop()
        {
            if(SceneNavigator.Instance != null)
                SceneNavigator.Instance.GoToShop();
        }

        public void GoToMine()
        {
            if(SceneNavigator.Instance != null)
                SceneNavigator.Instance.GoToMine();
        }
    }

}
