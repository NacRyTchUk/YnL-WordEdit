/*
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Chunkk
    {
        private Vector2 _coordOfTheChunk;

        List<Block> listOfBlocks = new List<Block>();

        GameObject parentOfChunk, chunkObject;

        public void SetCoord(int x, int y)
        {
            _coordOfTheChunk.x = x;
            _coordOfTheChunk.y = y;
        }

        public void SetCoord(Vector2 COORD)
        {
            _coordOfTheChunk = COORD;
        }

        public Vector2 GetCoord()
        {
            return _coordOfTheChunk;
        }

        public void DisplayInfo()
        {
            Debug.Log("Chunk x: " + _coordOfTheChunk.x + ", y: " + _coordOfTheChunk.y);
        }


        public void LoadInMemory()
        {
            TextAsset txtAsset = (TextAsset)Resources.Load("Map\\" + _coordOfTheChunk.x + "," + _coordOfTheChunk.y, typeof(TextAsset));
            string[] pageOfFile = txtAsset.text.Split('$');

            string[] bricksInFile = pageOfFile[0].Split(':');

            Block NewBlock;
            for (int i = 0; i < bricksInFile.Length; i++)
            {
                bricksInFile[i] = bricksInFile[i].Trim('[');
                bricksInFile[i] = bricksInFile[i].Trim(']');
                string[] elementsInBrick = bricksInFile[i].Split(';');
                NewBlock.blockIndex = Convert.ToInt32(elementsInBrick[0]);
                NewBlock.x = Convert.ToSingle(elementsInBrick[1]);
                NewBlock.y = Convert.ToSingle(elementsInBrick[2]);
                NewBlock.mainLayer = Convert.ToInt32(elementsInBrick[3]);
                listOfBlocks.Add(NewBlock);
            }

        }

        public void LoadOnScreen()
        {

            chunkObject = MonoBehaviour.Instantiate((GameObject)Resources.Load("Empty"), parentOfChunk.transform);
            chunkObject.name = _coordOfTheChunk.x + "," + _coordOfTheChunk.y;

            GameObject newGameObject;
            for (int i = 0; i < listOfBlocks.Count; i++)
            {
                newGameObject = MonoBehaviour.Instantiate((GameObject)Resources.Load("BlocksPrefabs/" + Convert.ToString(listOfBlocks[i].blockIndex)), chunkObject.transform);
                newGameObject.name = "[" + listOfBlocks[i].blockIndex + ";" + listOfBlocks[i].x + ";" + listOfBlocks[i].y + ";" + listOfBlocks[i].mainLayer + "]";
                newGameObject.transform.position = new Vector3(listOfBlocks[i].x * 0.5f, listOfBlocks[i].y * 0.5f, listOfBlocks[i].mainLayer * 0.5f);
                if (listOfBlocks[i].mainLayer == 0)
                    newGameObject.AddComponent<BoxCollider2D>();
            }
        }

        public void Load(GameObject POM)
        {
            parentOfChunk = POM;
            LoadInMemory();
            LoadOnScreen();
        }
    }
}
*/
