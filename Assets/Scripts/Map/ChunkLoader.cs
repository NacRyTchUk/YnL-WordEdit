using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

namespace Assets.Scripts.Map
{
    public class ChunkLoader : MonoBehaviour
    {
        private List<Chunk> _listOfChunks = new List<Chunk>();
        
        public List<Chunk> ListOfChunks
        {
            get { return _listOfChunks; }
            set { _listOfChunks = value; }
        }

        public void ddd()
        {
            Debug.Log(_listOfChunks);
        }
        public void LoadAllChunks()
        {
            
            foreach (var loc in _listOfChunks)
            {
                loc.Load(MapFileSystem.Map);
            }
            
            MapFileSystem.Map.transform.position = new Vector3(-0.5f,-0.5f);
        }


        public GameObject CreateNewChunk(Vector2 chunkPos)
        {
            var chunkName = string.Format("{0},{1}", chunkPos.x, chunkPos.y);
            var chunkHeader = MapFileSystem.Map.transform.Find(chunkName);
            if (chunkHeader != null) throw new Exception("Create new chunk: err");
             var newChunkHeader = Instantiate((GameObject) Resources.Load("Empty"), MapFileSystem.Map.transform);
             newChunkHeader.transform.position = new Vector3(chunkPos.x*Chunk.CHUNK_WIGHT,chunkPos.y*Chunk.CHUNK_HEIGHT);
             newChunkHeader.name = chunkName;
             _listOfChunks.Add(new Chunk(MapFileSystem.Map));
             return newChunkHeader;
        }

        public GameObject CreateNewLayer(int layerNumb,GameObject chunkHeader)
        {
            var layerName = string.Format("Layer: {0}",layerNumb); 
            var layerHeader = chunkHeader.transform.Find(layerName);
            if (layerHeader != null) throw new Exception("Create new chunk: err");
            var newLayerHeader = Instantiate((GameObject) Resources.Load("Empty"), chunkHeader.transform);
            newLayerHeader.name = layerName;

            return newLayerHeader;
        }
    }
}