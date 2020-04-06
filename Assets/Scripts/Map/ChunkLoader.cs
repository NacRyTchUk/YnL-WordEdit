using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

namespace Assets.Scripts.Map
{
    public class ChunkLoader : MonoBehaviour
    {
        public  const float GLOBAL_SPACE = -0.5f;
        
        private List<Chunk> _listOfChunks = new List<Chunk>();
        
        public List<Chunk> ListOfChunks
        {
            get { return _listOfChunks; }
            set { _listOfChunks = value; }
        }

        public void LoadAllChunks()
        {
            
            foreach (var loc in _listOfChunks)
            {
                loc.Load(MapFileSystem.Map);
            }
            
            MapFileSystem.Map.transform.position = new Vector3(+GLOBAL_SPACE,+GLOBAL_SPACE);
        }


        public GameObject CreateNewChunk(Vector2 chunkPos)
        {
            var chunkName = string.Format("{0},{1}", chunkPos.x, chunkPos.y);
            var chunkHeader = MapFileSystem.Map.transform.Find(chunkName);
            if (chunkHeader != null) throw new Exception("Create new chunk: err");
             var newChunkHeader = Instantiate((GameObject) Resources.Load("Empty"), MapFileSystem.Map.transform);
             newChunkHeader.transform.position = new Vector3(chunkPos.x*Chunk.CHUNK_WIGHT + GLOBAL_SPACE,chunkPos.y*Chunk.CHUNK_HEIGHT + GLOBAL_SPACE);
             newChunkHeader.name = chunkName;
             var newChunk = new Chunk(MapFileSystem.Map);
             newChunk.SetCoord(chunkPos);
             
             _listOfChunks.Add(newChunk);
             return newChunkHeader;
        }

        public GameObject CreateNewLayer(BlockPosition block,GameObject chunkHeader)
        {
            var layerName = string.Format("Layer: {0}",block.layer); 
            var layerHeader = chunkHeader.transform.Find(layerName);
            if (layerHeader != null) throw new Exception("Create new chunk: err");
            var newLayerHeader = Instantiate((GameObject) Resources.Load("Empty"), chunkHeader.transform);
            newLayerHeader.name = layerName;
            
            FindTheChunk(block.chunkPos).LayersHeaders[block.layer + Chunk.MIN_LAYER_VALUE] = newLayerHeader;
            return newLayerHeader;
        }


        private Chunk FindTheChunk(Vector2 chunkPos)
        {
            return _listOfChunks.FirstOrDefault(ch => ch.GetCoord() == chunkPos);
        }
    }
}