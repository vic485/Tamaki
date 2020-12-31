using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using VndbSharp;
using VndbSharp.Models;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var client = new Vndb(true);

        var result = client.GetVisualNovelAsync(VndbFilters.Id.Equals(20), VndbFlags.FullVisualNovel).Result;

        foreach (var visualNovel in result)
        {
            StartCoroutine(FileManager.DownloadCover(visualNovel));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
