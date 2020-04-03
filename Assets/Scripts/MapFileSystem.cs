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
        // ReSharper disable UnassignedField.Global // ReSharper disable MemberCanBePrivate.Global
        public Button buttonLoad, buttonCreateNew;
        public InputField inputBox;
        // ReSharper restore UnassignedField.Global // ReSharper restore MemberCanBePrivate.Global
        
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
            
            var cm = new ChunkManager {ListOfChunks = LoadChunksFromDer(inputBox.text)};
            cm.LoadAllChunks();
        }

        private void LoadMapInfoData()
        {
            
            
            
        }

        private List<Chunk> LoadChunksFromDer(string dir)
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


    public class ChunkManager
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
       
        private const int CHUNK_WIGHT = 48, CHUNK_HEIGHT = 24;
        private Vector2 _coordOfTheChunk;
        private string _dir;
        private List<Block> _listOfBlocks = new List<Block>();
        
        private  GameObject _parentOfChunk, _chunkObject;
        
        public void SetCoord(int x, int y)
        {
            _coordOfTheChunk.x = x;
            _coordOfTheChunk.y = y;
        }

        public string Dir
        {
            get { return _dir; }
            set { _dir = value; }
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
            var pic = MapFileSystem.pic;
            _chunkObject = Object.Instantiate((GameObject)Resources.Load("Empty"), _parentOfChunk.transform);
            _chunkObject.name = _coordOfTheChunk.x + "," + _coordOfTheChunk.y;

            foreach (var lob in _listOfBlocks)
            {
               
                var newObj = Object.Instantiate((GameObject)Resources.Load("Empty"), _chunkObject.transform);
                newObj.name = "[" + lob.blockIndex + ";" + lob.x + ";" + lob.y + ";" + lob.mainLayer + "]";
                
                newObj.AddComponent<SpriteRenderer>().sprite = pic[lob.blockIndex ];
                
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
