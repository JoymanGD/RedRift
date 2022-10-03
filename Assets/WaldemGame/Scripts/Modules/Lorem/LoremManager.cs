using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using WaldemGame.Managers;

namespace WaldemGame.Lorem
{
    public class LoremManager : AManager
    {
        private Sprite[] sprites;
        private const string BASE_URL = "https://picsum.photos";

        public async Task<Sprite> GetRandomImage(float sizeX, float sizeY)
        {
            Sprite result = null;

            var url = BASE_URL + "/" + $"{sizeX}/{sizeY}.jpg";

            var texture = await GetLoremTexture(url);

            
            result = Sprite.Create(texture, new Rect(0, 0, sizeX, sizeY), new Vector2(.5f, .5f));

            return result;
        }

        public async Task<List<Sprite>> GetRandomImages(int amount, float sizeX, float sizeY)
        {
            List<Sprite> result = new List<Sprite>();

            var url = BASE_URL + "/" + $"{sizeX}/{sizeY}.jpg";

            List<Texture2D> textures = await GetLoremTextures(amount, url);

            foreach (var item in textures)
            {
                var sprite = Sprite.Create(item, new Rect(0, 0, sizeX, sizeY), new Vector2(.5f, .5f));
                
                result.Add(sprite);
            }

            return result;
        }

        public static async Task<Texture2D> GetLoremTexture(string url)
        {
            Debug.Log("UnityWebRequestTexture creating started");
            using(UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                Debug.Log("UnityWebRequestTexture created");
                var asyncOp = request.SendWebRequest();
                Debug.Log("UnityWebRequestTexture sent");

                while(!asyncOp.isDone)
                {
                    Debug.Log("Checking if asyncOp.isDone in a while loop");
                    await Task.Delay(1000/30);
                }
                
                if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.Log($"{request.error}, URL:{request.url}");
                    
                    return null;
                }
                else
                {
                    var texture = DownloadHandlerTexture.GetContent(request);
                    return texture;
                }
            }
        }

        public static async Task<List<Texture2D>> GetLoremTextures(int amount, string url)
        {
            List<Texture2D> result = new List<Texture2D>();
            UnityWebRequest request = null;

            for (int i = 0; i < amount; i++)
            {
                request = UnityWebRequestTexture.GetTexture(url);
                var asyncOp = request.SendWebRequest();

                while(!asyncOp.isDone)
                {
                    await Task.Delay(1000/30);
                }
                
                if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.DataProcessingError)
                {
                    #if DEBUG
                    Debug.Log($"{request.error}, URL:{request.url}");
                    #endif
                    
                    return null;
                }
                else
                {
                    var texture = DownloadHandlerTexture.GetContent(request);
                    result.Add(texture);

                    request.Abort();
                    request.Dispose();
                }
            }

            return result;
        }
    }
}