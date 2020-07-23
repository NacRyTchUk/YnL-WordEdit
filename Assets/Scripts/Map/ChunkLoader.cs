using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

namespace Map
{
    public class ChunkLoader : MonoBehaviour
    {
        public  const float GlobalSpace = -0.5f;

        public List<Chunk> ListOfChunks { get; set; } = new List<Chunk>();

        public void LoadAllChunks()
        {
            
            foreach (var loc in ListOfChunks)
            {
                loc.Load(MapFileSystem.Map);
            }
            
            MapFileSystem.Map.transform.position = new Vector3(+GlobalSpace,+GlobalSpace);
        }


        public GameObject CreateNewChunk(Vector2 chunkPos)
        {
            string chunkName = $"{chunkPos.x},{chunkPos.y}";
            var chunkHeader = MapFileSystem.Map.transform.Find(chunkName);
            if (chunkHeader != null) throw new Exception("Create new chunk: err");
             var newChunkHeader = Instantiate((GameObject) Resources.Load("Empty"), MapFileSystem.Map.transform);
             newChunkHeader.transform.position = new Vector3(chunkPos.x*Chunk.ChunkWight + GlobalSpace,chunkPos.y*Chunk.ChunkHeight + GlobalSpace);
             newChunkHeader.name = chunkName;
             var newChunk = new Chunk();
             newChunk.SetCoords(chunkPos);
             
             ListOfChunks.Add(newChunk);
             return newChunkHeader;
        }

        public GameObject CreateNewLayer(BlockPosition block,GameObject chunkHeader)
        {
            string layerName = $"Layer: {block.layer}"; 
            var layerHeader = chunkHeader.transform.Find(layerName);
            if (layerHeader != null) throw new Exception("Create new chunk: err");
            var newLayerHeader = Instantiate((GameObject) Resources.Load("Empty"), chunkHeader.transform);
            newLayerHeader.name = layerName;
            
            FindTheChunk(block.chunkPos).LayersHeaders[block.layer + Chunk.MinLayerValue] = newLayerHeader;
            return newLayerHeader;
        }


        private Chunk FindTheChunk(Vector2 chunkPos)
        {
            return ListOfChunks.FirstOrDefault(ch => ch.GetCoords() == chunkPos);
        }
    }
}