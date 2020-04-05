using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

namespace Assets.Scripts
{
    public class ChunkLoader : MonoBehaviour
    {
        private List<Chunk> _listOfChunks = new List<Chunk>();
        private GameObject _map;
        
        public List<Chunk> ListOfChunks
        {
            get { return _listOfChunks; }
            set { _listOfChunks = value; }
        }

        public void LoadAllChunks()
        {
            _map = new GameObject("Map");
            foreach (var loc in _listOfChunks)
            {
                loc.Load(_map);
            }
            
            _map.transform.position = new Vector3(-0.5f,-0.5f);
        }

    }
}