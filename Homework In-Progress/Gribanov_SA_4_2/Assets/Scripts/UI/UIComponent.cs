using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid.UI
{
    public class UIComponent : MonoBehaviour
    {
        private Text _healthT;
        private void Awake()
        {
            _healthT = gameObject.GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            _healthT.text = $"{Managers.GameManager.Self.GetHealth()}";
        }
    }
}
