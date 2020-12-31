using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using VndbSharp.Models.VisualNovel;

namespace Managers
{
    public static class FileManager
    {
        private static readonly string ImagePath = Path.Combine(Application.persistentDataPath, "images");

        static FileManager()
        {
            var coverPath = Path.Combine(ImagePath, "covers");
            if (!Directory.Exists(coverPath))
                Directory.CreateDirectory(coverPath);

            var screenPath = Path.Combine(ImagePath, "screenshots");
            if (!Directory.Exists(screenPath))
                Directory.CreateDirectory(screenPath);
        }

        public static IEnumerator DownloadCover(VisualNovel vn)
        {
            // TODO: Check image rating against global setting
            if (vn.ImageRating.SexualAvg > 1)
                yield break;

            var www = UnityWebRequest.Get(vn.Image);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var fileName = $"{vn.Id}{Path.GetExtension(vn.Image)}";
                var path = Path.Combine(ImagePath, "covers", fileName);
                File.WriteAllBytes(path, www.downloadHandler.data);
            }
        }
    }
}
