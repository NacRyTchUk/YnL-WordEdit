using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    public class Chunk
    {
        public  const int CHUNK_WIGHT = 48, CHUNK_HEIGHT = 24, MIN_LAYER_VALUE = 5, MAX_LAYER_VALUE = 5;
        private GameObject[] _layersHeaders = new GameObject[MAX_LAYER_VALUE + MIN_LAYER_VALUE + 1];
        private Vector2 _coordOfTheChunk;
        private string _dir;
        private List<Block> _listOfBlocks = new List<Block>();

        private GameObject _parentOfChunk;


        public string Dir
        {
            get { return _dir; }
            set { _dir = value; }
        }

        public static int MinLayerValue
        {
            get { return MIN_LAYER_VALUE; }
        }

        public GameObject[] LayersHeaders
        {
            get { return _layersHeaders; }
        }


        public void SetCoord(int x, int y)
        {
            _coordOfTheChunk.x = x;
            _coordOfTheChunk.y = y;
        }

        public void SetCoord(Vector2 coord)
        {
            _coordOfTheChunk = coord;
        }

        public Vector2 GetCoord()
        {
            return _coordOfTheChunk;
        }

        private void LoadOnScreen()
        {
            var chunkHeader = Object.Instantiate((GameObject) Resources.Load("Empty"), _parentOfChunk.transform);
            chunkHeader.transform.position = new Vector3(_coordOfTheChunk.x*CHUNK_WIGHT,_coordOfTheChunk.y*CHUNK_HEIGHT);
            chunkHeader.name = _coordOfTheChunk.x + "," + _coordOfTheChunk.y;

            foreach (var lob in _listOfBlocks)
            {
                
                var layerValue = lob.mainLayer + MIN_LAYER_VALUE;
                if (_layersHeaders[layerValue] == null)
                {
                    _layersHeaders[layerValue] =
                        Object.Instantiate((GameObject) Resources.Load("Empty"), chunkHeader.transform);
                    _layersHeaders[layerValue].name = "Layer: " + lob.mainLayer;
                }

                var newObj = Object.Instantiate((GameObject) Resources.Load("Empty"),
                    _layersHeaders[layerValue].transform);
                newObj.name = "[" + lob.blockIndex + ";" + lob.x + ";" + lob.y + ";" + lob.mainLayer + "]";

                newObj.AddComponent<SpriteRenderer>().sprite = MapFileSystem.pic[lob.blockIndex];

                newObj.transform.position += new Vector3(lob.x, lob.y, lob.mainLayer);
            }
        }


        private void LoadInMemory()
        {
            using (var sr = new StreamReader(_dir + "/" + _coordOfTheChunk.x + ',' + _coordOfTheChunk.y + ".chunk"))
            {
                var fileLine = sr.ReadLine();
                if (fileLine == null) throw new NullReferenceException();

                var mainBlock = fileLine.Split('$');
                var bricks = mainBlock[0].Split(':');
                foreach (var t in bricks)
                {
                    var oneBrick = t.Replace("[", "").Replace("]", "");
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

        public void Load([NotNull] GameObject pom)
        {
            _parentOfChunk = pom;
            LoadInMemory();
            LoadOnScreen();
        }
    }
}
