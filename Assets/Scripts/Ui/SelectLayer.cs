using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
	public class SelectLayer : MonoBehaviour
	{
		[SerializeField] private GameObject eventObj;


		public void AllVisible()
		{
			LayerVisibleSet(-Chunk.MIN_LAYER_VALUE - 1);
		}
		
		public void IncBtn(InputField inputField)
		{
			InputBoxChangeValue(inputField,1);
		}

		public void DecBtn(InputField inputField)
		{
			InputBoxChangeValue(inputField,-1);
		} 
		

		private void InputBoxChangeValue(InputField inputField,int value)
		{
			var intIF = Convert.ToInt32(inputField.text);
			inputField.text = Convert.ToString(intIF + value);
			LayerVisibleSet(Convert.ToInt32(inputField.text));
			
		}
		
		
		private void LayerVisibleSet(int layerVisible)
		{
			Debug.Log(layerVisible);
			var cl = eventObj.GetComponent<MapFileSystem>().GetChunkLoader;
			var loc = cl.ListOfChunks;
			foreach (var ch in loc)
			{
				for (var i = 0; i < ch.LayersHeaders.Length; i++)
				{
					if (ch.LayersHeaders[i] == null) continue;
					ch.LayersHeaders[i].SetActive((i == layerVisible + Chunk.MIN_LAYER_VALUE) || (layerVisible < -Chunk.MIN_LAYER_VALUE)); // for fix 
				}
			}
		}
	}
}
