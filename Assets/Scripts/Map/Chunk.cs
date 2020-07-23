using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Map
{
    public class Chunk
    {
        public  const int ChunkWight = 48, ChunkHeight = 24, MinLayerValue = 5;
        private const int MaxLayerValue = 5;
        private Vector2 _coordOfTheChunk;
        private readonly List<Block> _listOfBlocks = new List<Block>();

        public Chunk()
        {
        }

        

        public string Dir { get; set; }
        

        public GameObject[] LayersHeaders { get; } = new GameObject[MaxLayerValue + MinLayerValue + 1];

        private GameObject ParentOfChunk { get; set; }


        public void SetCoords(int x, int y)
        {
            _coordOfTheChunk.x = x;
            _coordOfTheChunk.y = y;
        }

        public void SetCoords(Vector2 coords)
        {
            _coordOfTheChunk = coords;
        }

        public Vector2 GetCoords()
        {
            return _coordOfTheChunk;
        }

        private void LoadOnScreen()
        {
            
            var chunkHeader = Object.Instantiate((GameObject) Resources.Load("Empty"), ParentOfChunk.transform);
            chunkHeader.transform.position = new Vector3(_coordOfTheChunk.x*ChunkWight,_coordOfTheChunk.y*ChunkHeight);
            chunkHeader.name = _coordOfTheChunk.x + "," + _coordOfTheChunk.y;

            foreach (var lob in _listOfBlocks)
            {
                
                int layerValue = lob.mainLayer + MinLayerValue;
                if (LayersHeaders[layerValue] == null)
                {
                    LayersHeaders[layerValue] =
                        Object.Instantiate((GameObject) Resources.Load("Empty"), chunkHeader.transform);
                    LayersHeaders[layerValue].name = "Layer: " + lob.mainLayer;
                }

                var newObj = Object.Instantiate((GameObject) Resources.Load("Empty"),
                    LayersHeaders[layerValue].transform);
                newObj.name = "[" + lob.blockIndex + ";" + lob.x + ";" + lob.y + ";" + lob.mainLayer + "]";

                newObj.AddComponent<SpriteRenderer>().sprite = MapFileSystem.pic[lob.blockIndex];

                newObj.transform.position += new Vector3(lob.x, lob.y, lob.mainLayer);
            }
        }


        private void LoadInMemory()
        {
            using (var sr = new StreamReader(Dir + "/" + _coordOfTheChunk.x + ',' + _coordOfTheChunk.y + ".chunk"))
            {
                string fileLine = sr.ReadLine();
                if (fileLine == null) throw new NullReferenceException();

                var mainBlock = fileLine.Split('$');
                var bricks = mainBlock[0].Split(':');
                foreach (var t in bricks)
                {
                    string oneBrick = t.Replace("[", "").Replace("]", "");
                    var brickData = oneBrick.Split(';');

                    Block newBlock;
                    newBlock.blockIndex = Convert.ToInt32(brickData[0]);
                    newBlock.x = Convert.ToSingle(brickData[1]);
                    newBlock.y = Convert.ToSingle(brickData[2]);
                    newBlock.mainLayer = Convert.ToInt32(brickData[3]);
                    _listOfBlocks.Add(newBlock);
                }
            }
        }

        public void Load([NotNull] GameObject poc)
        {
            ParentOfChunk = poc;
            LoadInMemory();
            LoadOnScreen();
        }
    }
}
