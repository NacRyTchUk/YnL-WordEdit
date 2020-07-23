using System;
using Map;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
	public class SelectLayer : MonoBehaviour
	{
		public int Layer { get; private set; }

		public void AllVisible()
		{
			LayerVisibleSet(-Chunk.MinLayerValue - 1);
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
			int intIf = Convert.ToInt32(inputField.text);
			inputField.text = Convert.ToString(intIf + value);
			Layer = Convert.ToInt32(inputField.text);
			LayerVisibleSet(Layer);
		}
		
		
		private static void LayerVisibleSet(int layerVisible)
		{
			Debug.Log(layerVisible);
			var cl = MapFileSystem.GetChunkLoader;
			var loc = cl.ListOfChunks;
			foreach (var ch in loc)
			{
				for (int i = 0; i < ch.LayersHeaders.Length; i++)
				{
					if (ch.LayersHeaders[i] == null) continue;
					ch.LayersHeaders[i].SetActive((i == layerVisible + Chunk.MinLayerValue) || (layerVisible < -Chunk.MinLayerValue)); // for fix 
				}
			}
		}
	}
}
