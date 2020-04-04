using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

namespace Assets.Scripts
{
    public class ChunkLoader
    {
        private List<Chunk> _listOfChunks = new List<Chunk>();
        private GameObject map = new GameObject("Map");
        
        public List<Chunk> ListOfChunks
        {
            get { return _listOfChunks; }
            set { _listOfChunks = value; }
        }

        public void LoadAllChunks()
        {
            foreach (var loc in _listOfChunks)
            {
                loc.Load(map);
            }
            
        }

    }
}