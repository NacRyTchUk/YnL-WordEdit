using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{    
    
    struct Block
    {
        public float x;
        public float y;
        public int mainLayer;
        public int blockIndex;
    }

    struct Map
    {
        public List<Chunk> chunks;
    }
    public class MapFileSystem  : MonoBehaviour
    {
        
        

        public static  Sprite[] pic;
        
        [SerializeField] public Button buttonLoad;
        [SerializeField] public Button buttonCreateNew;
        [SerializeField] public InputField inputBox;
        
        
        private string _mapNumbPostfix;
        
        private void Start()
        {
            var counter = 0;
            while (true)
            {
                counter++;
                if (File.Exists(Application.dataPath + "/Maps/New Map" + _mapNumbPostfix + "/Map.info"))
                    _mapNumbPostfix = " (" + counter + ")";
                else
                    break;
            }
            inputBox.text = Application.dataPath + "/Maps/New Map" + _mapNumbPostfix;


            pic = LoadSprites(Application.streamingAssetsPath, "*.png");
        }

        public void LoadTheMap()
        {
            
            var cm = new ChunkLoader {ListOfChunks = LoadChunksFrom(inputBox.text)};
            cm.LoadAllChunks();
        }

        private void LoadMapInfoData()
        {
            
            
            
        }

        private static List<Chunk> LoadChunksFrom(string dir)
        {
            var tChunkList = Directory.GetFiles(dir, "*.chunk");
            var tMap = new List<Chunk>();

            foreach (var ch in tChunkList)
            {
                var tStr = ch.Replace(dir + "\\", "").Replace(".chunk", "").Split(',');
                var xIndex = Convert.ToInt32(tStr[0]);
                var yIndex = Convert.ToInt32(tStr[1]);
                
                var newChunk = new Chunk();
                newChunk.SetCoord(xIndex,yIndex);
                newChunk.Dir = dir;
                tMap.Add(newChunk);
            }

            return tMap;
        }

        
    
        public static void CreateNewMap()
        {
            
        }




        private static Sprite[] LoadSprites(string dir, string filter)
        {
            var filePaths = Directory.GetFiles(dir, filter);
            var fromTex = new Sprite[filePaths.Length];
        
            for (var i = 0; i < filePaths.Length; i++)
            {
                var pngBytes = File.ReadAllBytes(filePaths[i]);
                var tex = new Texture2D(5,5);
                tex.LoadImage(pngBytes);
                fromTex[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 64);
            }
            return fromTex;
        }




    }


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
    
    public class Chunk 
    {
       
        private const int CHUNK_WIGHT = 48, CHUNK_HEIGHT = 24, MIN_LAYER_VALUE = 5,MAX_LAYER_VALUE = 5;
        private Vector2 _coordOfTheChunk;
        private string _dir;
        private List<Block> _listOfBlocks = new List<Block>();
        
        private  GameObject _parentOfChunk;
        
        

        public string Dir
        {
            get { return _dir; }
            set { _dir = value; }
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
            var chunkHeader = Object.Instantiate((GameObject)Resources.Load("Empty"), _parentOfChunk.transform);
            chunkHeader.name = _coordOfTheChunk.x + "," + _coordOfTheChunk.y;
            
            var layersHeaders = new GameObject[MAX_LAYER_VALUE+MIN_LAYER_VALUE+1];
            
            foreach (var lob in _listOfBlocks)
            {
                var layerValue = lob.mainLayer + MIN_LAYER_VALUE;
                if (layersHeaders[layerValue] == null)
                {
                    layersHeaders[layerValue] = Object.Instantiate((GameObject) Resources.Load("Empty"),chunkHeader.transform); 
                    layersHeaders[layerValue].name = "Layer: " + lob.mainLayer;
                }

                var newObj = Object.Instantiate((GameObject)Resources.Load("Empty"), layersHeaders[layerValue].transform);
                newObj.name = "[" + lob.blockIndex + ";" + lob.x + ";" + lob.y + ";" + lob.mainLayer + "]";
                
                newObj.AddComponent<SpriteRenderer>().sprite = MapFileSystem.pic[lob.blockIndex];
                
                newObj.transform.position = new Vector3(lob.x + _coordOfTheChunk.x*CHUNK_WIGHT, lob.y + _coordOfTheChunk.y*CHUNK_HEIGHT, lob.mainLayer);
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
