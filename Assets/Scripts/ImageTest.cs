using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageTest : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        var image = GetComponent<Image>();
        using (var imageRequest =
            UnityWebRequestTexture.GetTexture(Path.Combine($"file://{Application.persistentDataPath}", "images", "covers", "20.jpg")))
        {
            yield return imageRequest.SendWebRequest();

            if (imageRequest.isHttpError || imageRequest.isNetworkError)
            {
                Debug.Log(imageRequest.error);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(imageRequest);
                image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                image.preserveAspect = true;
                image.gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
