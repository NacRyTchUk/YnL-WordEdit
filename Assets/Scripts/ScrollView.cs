using System;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global
// ReSharper disable IdentifierTypo

namespace Assets.Scripts
{
    public class ScrollView : MonoBehaviour {
        
        public GameObject content;
         public Scrollbar vScroolBar;

       public GameObject layoutElement;

        private RectTransform _conternRt, _scrollBarRt, _elemlRt;
        private int _colons;

        


        private void Start()
        {
            _conternRt = content.GetComponent<RectTransform>();
            _scrollBarRt = vScroolBar.GetComponent<RectTransform>();
            _elemlRt = layoutElement.GetComponent<RectTransform>();
        }

        private static double RoundDown(double number, int p)
        {
            return Math.Round(number - number % Math.Pow(10, p));
        }

        public void OnResize()
        {
            _colons = Convert.ToInt32(RoundDown((_conternRt.rect.width) / _elemlRt.rect.width, 0));
            _colons = Convert.ToInt32(RoundDown((_conternRt.rect.width + _conternRt.GetComponent<GridLayoutGroup>().spacing.x*_colons) / _elemlRt.rect.width, 0));
            Debug.Log(_colons);
        }

    }
}
