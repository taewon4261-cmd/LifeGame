using LifeGame.UI;
using System.Collections.Generic;
using UnityEngine;

namespace LifeGame.UI
{
    public class SceneConfig : MonoBehaviour
    {
        public List<ButtonData> sceneButtons;

        void Start()
        {

            if (ControlPanelController.Instance != null)
            {
                ControlPanelController.Instance.InitializeButtons(sceneButtons);
            }
            else
            {
                Debug.LogError("ControlPanelController가 씬에 없습니다! 프리팹을 배치했나요?");
            }
        }
    }
}
