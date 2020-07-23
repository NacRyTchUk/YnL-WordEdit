using UnityEngine;

namespace Map
{
	public class BlockManager : MonoBehaviour
	{
		//[SerializeField] private ChunkLoader cl;
		
		public void BlockPlace(BlockPosition block)
		{
			Debug.Log("testBlockPlace");
			if (IsPlaceFree(block))
			{
				CreateBlock(block);
				
				Debug.Log("blockCreated");
			} else 
				
				Debug.Log("ErrCreated");
		}

		private bool IsPlaceFree(BlockPosition block)
		{
			var chunkHeader = GameObject.Find(GetChunkName(block));
			if (chunkHeader == null) return true;
			
			Transform layerHeader = chunkHeader.transform.Find(GetLayerName(block));
			if (layerHeader == null) return true;
			
			var blockObj = layerHeader.transform.Find(GetBlockName(block));
			Debug.Log(layerHeader.transform.Find(GetBlockName(block)));
			return blockObj == null;

		}

		private Transform GetPathToLayer(BlockPosition block)
		{
			var cl = MapFileSystem.GetChunkLoader;
			var chunkHeader = GameObject.Find(GetChunkName(block));
			if (chunkHeader == null)
			{
				chunkHeader = cl.CreateNewChunk(block.chunkPos);
			}
			
			var layerHeader = chunkHeader.transform.Find(GetLayerName(block));
			if (layerHeader == null) layerHeader = cl.CreateNewLayer(block,chunkHeader).transform;
			
			return layerHeader;
		}
		private void CreateBlock(BlockPosition block)
		{
			const int blockIndex = 1;
			var layerHeader = GetPathToLayer(block);
			var newBlock = Instantiate((GameObject) Resources.Load("Empty"), layerHeader.transform);
			newBlock.transform.position += new Vector3(block.blockPos.x,block.blockPos.y);
			newBlock.name = "[" + blockIndex + ";" + block.blockPos.x + ";" + block.blockPos.y + ";" + block.layer + "]";
			newBlock.AddComponent<SpriteRenderer>().sprite = MapFileSystem.pic[blockIndex];

		}

		private static string GetBlockName(BlockPosition block)
		{
			const int blockIndex = 1;
			string result = $"[{blockIndex};{block.blockPos.x};{block.blockPos.y};{block.layer}]";
			return result;
		}

		private static string GetChunkName(BlockPosition block)
		{
			string result = $"{block.chunkPos.x},{block.chunkPos.y}"; 
			return result;
		}

		private string GetLayerName(BlockPosition block)
		{
			string result = $"Layer: {block.layer}"; 
			return result;
		}
	}
}
