using UnityEngine;

namespace Assets.Scripts.Map
{
	public class BlockManager : MonoBehaviour
	{
		//[SerializeField] private ChunkLoader cl;
		
		public void BlockPlace(BlockPosition block)
		{
			Debug.Log("testBlockPlace");
			if (isPlaceFree(block))
			{
				CreateBlock(block);
				
				Debug.Log("blockCreated");
			} else 
				
				Debug.Log("ErrCreated");
		}

		private bool isPlaceFree(BlockPosition block)
		{
			var chunkHeader = GameObject.Find(GetChunkName(block));
			if (chunkHeader == null) return true;
			
			var layerHeader = chunkHeader.transform.Find(GetLayerName(block));
			if (layerHeader == null) return true;
			
			var blockObj = layerHeader.transform.Find(GetBlockName(block));
			Debug.Log(layerHeader.transform.Find(GetBlockName(block)));
			return blockObj == null;

		}

		private Transform GetPathToLayer(BlockPosition block)
		{
			var cl = MapFileSystem.GetChunkLoader;
			var chunkHeader = GameObject.Find(GetChunkName(block));
			if (chunkHeader == null) chunkHeader = cl.CreateNewChunk(block.chunkPos);
			var ch = new Chunk(MapFileSystem.Map);
			
			
			var layerHeader = chunkHeader.transform.Find(GetLayerName(block)).gameObject;
			if (layerHeader == null) layerHeader = cl.CreateNewLayer(block.layer,chunkHeader);
			ch.LayersHeaders[block.layer + Chunk.MIN_LAYER_VALUE] = layerHeader;
			
			var loc = cl.ListOfChunks;
			loc.Add(ch);
			cl.ListOfChunks = loc;
			return layerHeader.transform;
		}
		private void CreateBlock(BlockPosition block)
		{
			var blockIndex = 1;
			var layerHeader = GetPathToLayer(block);
			var newBlock = Instantiate((GameObject) Resources.Load("Empty"), layerHeader.transform);
			newBlock.transform.position += new Vector3(block.blockPos.x,block.blockPos.y);
			newBlock.name = "[" + blockIndex + ";" + block.blockPos.x + ";" + block.blockPos.y + ";" + block.layer + "]";
			newBlock.AddComponent<SpriteRenderer>().sprite = MapFileSystem.pic[blockIndex];

		}

		private string GetBlockName(BlockPosition block)
		{
			var blockIndex = 1;
			var result = string.Format("[{0};{1};{2};{3}]",blockIndex,block.blockPos.x,block.blockPos.y,block.layer);
			return result;
		}

		private string GetChunkName(BlockPosition block)
		{
			var result = string.Format("{0},{1}",block.chunkPos.x,block.chunkPos.y); 
			return result;
		}

		private string GetLayerName(BlockPosition block)
		{
			var result = string.Format("Layer: {0}",block.layer); 
			return result;
		}
	}
}
