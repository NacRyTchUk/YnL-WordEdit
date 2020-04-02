using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MapLoader  : MonoBehaviour
    {
        private struct Block
        {
            public float x;
            public float y;
            public int mainLayer;
            public int blockIndex;
        }


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
        }

        public void LoadTheMap()
        {
            Vector2 startCoord;
            var listOfSprites = new List<Block>();

            using (var sr = new StreamReader(inputBox.text + "/Map.info"))
            {
                var fileLine = sr.ReadLine();
                if (fileLine == null) return;
                var tempCoord = fileLine.Split(';');
                startCoord.x = Convert.ToInt32(tempCoord[0]);
                startCoord.y = Convert.ToInt32(tempCoord[1]);

                 
            }
            using (var sr = new StreamReader(inputBox.text + "/" + startCoord.x + ',' + startCoord.y + ".chunk"))
            {
                
                var pic = LoadSprites(Application.streamingAssetsPath, "*.png");
                
                var fileLine = sr.ReadLine();
                if (fileLine == null) return;
                
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
                    listOfSprites.Add(newBlock);
                    var spr = new GameObject();
                    spr.AddComponent<SpriteRenderer>().sprite = pic[newBlock.blockIndex ];



                    spr.transform.position = new Vector3(newBlock.x, newBlock.y, newBlock.mainLayer);
                }
            }
            StartUiDissapers(true);
        }

        private void StartUiDissapers(bool yor) 
        {
            if (yor)
            {
                buttonLoad.gameObject.SetActive(false);
                buttonCreateNew.gameObject.SetActive(false);
                inputBox.gameObject.SetActive(false);
            }
            else
            {

                buttonLoad.gameObject.SetActive(true);
                buttonCreateNew.gameObject.SetActive(true);
                inputBox.gameObject.SetActive(true);
            }
        }
    
        public void CreateNewMap()
        {
            
        }




        private static Sprite[] LoadSprites(string dir, string filter)
        {
            var filePaths = Directory.GetFiles(dir, filter);
            var fromTex = new Sprite[filePaths.Length];
        
            for (var i = 0; i < filePaths.Length; i++)
            {
                var pngBytes = System.IO.File.ReadAllBytes(filePaths[i]);
                var tex = new Texture2D(5,5);
                tex.LoadImage(pngBytes);
                fromTex[i] = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 64);
            }
            return fromTex;
        }




    }

    
}
