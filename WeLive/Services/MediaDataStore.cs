using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Collections.Generic;
namespace WeLive
{
    public class MediaDataStore:BaseDataStore
    {
        public MediaDataStore()
        {
        }
        public async Task<string> UploadImage(string path)
        {
            string url = "api/properties/uploadImage";
            var ImageDate = File.ReadAllBytes(path);
			var requestContent = new MultipartFormDataContent();
			var imageContent = new ByteArrayContent(ImageDate);
			imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
			requestContent.Add(imageContent, "image", Guid.NewGuid().ToString() + ".jpg");
            var response = await client.PostAsync(url , requestContent);
			var stringContent = await response.Content.ReadAsStringAsync();
            stringContent = stringContent.Trim().Trim('"').Replace("\\/", "/");
            stringContent = App.BackendUrl + stringContent;
			return stringContent;
        }
    }
}
