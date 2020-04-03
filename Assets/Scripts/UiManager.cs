using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public enum UiElementsIndex {LoadButt, CreateButt, InputField};
	public enum UiModeIndex {MainMenu, Editor, BlockSelecting};
	
	
	
	
	public  class UiManager : MonoBehaviour
	{
		public  UiModeIndex curModeOfUi = UiModeIndex.MainMenu;
		public GameObject[] headerOfUi = new GameObject[]{};
		public void ChangeUiVisible(UiModeIndex umi)
		{
			DisableUi(curModeOfUi);
			EnableUi(umi);
			curModeOfUi = umi;
		}

		
		
		private void DisableUi(UiModeIndex umi)
		{
			headerOfUi[(int)umi].SetActive(false);
		}
		
		private void EnableUi(UiModeIndex umi)
		{
			headerOfUi[(int)umi].SetActive(true);
		}
	

	}
}
