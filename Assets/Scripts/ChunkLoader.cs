/*using System;
using System.IO;
using UnityEngine;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

namespace Assets.Scripts
{
    public class ChunkLoader : MonoBehaviour
    {
        public GameObject parentsOfChunks;
         
        
        
        public void LoadTheChunk(Chunk curChunk)
        {    
            var chunkCoord = curChunk.GetCoord();
            

           
            using (var sr = new StreamReader(inputBox.text + "/" + chunkCoord.x + ',' + chunkCoord.y + ".chunk"))
            {
                
                var pic = LoadSprites(Application.streamingAssetsPath, "*.png");
                
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
                    
                    
                    var spr = new GameObject();
                    spr.AddComponent<SpriteRenderer>().sprite = pic[newBlock.blockIndex ];

                    spr.transform.position = new Vector3(newBlock.x, newBlock.y, newBlock.mainLayer);
                }
            }
            GetComponent<UiManager>().ChangeUiVisible(UiModeIndex.Editor);
        }
        
        
    }
}*/