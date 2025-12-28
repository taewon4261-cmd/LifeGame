using LifeGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LifeGame.Manager
{
    public class TitleManager : MonoBehaviour
    {
        public void GoToTown()
        {
            SceneManager.LoadScene("Town");
        }
    }

}
