using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Map;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    public struct Block
    {
        public float x;
        public float y;
        public int mainLayer;
        public int blockIndex;
    }

    public class MapFileSystem : MonoBehaviour
    {
        public static Sprite[] pic;

        [SerializeField] private Button _buttonLoad;
        [SerializeField] private Button _buttonCreateNew;
        [SerializeField] private InputField _inputBox;

        private ChunkLoader _chunkLoader;
        private string _mapNumbPostfix;
        private static GameObject _map;
        public ChunkLoader GetChunkLoader
        {
            get { return _chunkLoader; }
        }

        public InputField InputBox
        {
            get { return _inputBox; }
        }

        public static GameObject Map
        {
            get { return _map; }
        }

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

            InputBox.text = Application.dataPath + "/Maps/New Map" + _mapNumbPostfix;


            pic = LoadSprites(Application.streamingAssetsPath, "*.png");
        }

        public void LoadTheMap()
        {
            _chunkLoader = new ChunkLoader {ListOfChunks = LoadChunksFrom(InputBox.text)};
            GetChunkLoader.LoadAllChunks();
        }

        private void LoadMapInfoData()
        {
        }

        private static List<Chunk> LoadChunksFrom(string dir)
        {
            _map = new GameObject("Map");
            var tChunkList = Directory.GetFiles(dir, "*.chunk");
            var tMap = new List<Chunk>();

            foreach (var ch in tChunkList)
            {
                var tStr = ch.Replace(dir + "\\", "").Replace(".chunk", "").Split(',');
                var xIndex = Convert.ToInt32(tStr[0]);
                var yIndex = Convert.ToInt32(tStr[1]);

                var newChunk = new Chunk(Map);
                newChunk.SetCoord(xIndex, yIndex);
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
                var tex = new Texture2D(5, 5);
                tex.LoadImage(pngBytes);
                fromTex[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 64);
            }

            return fromTex;
        }
    }
    
    
}