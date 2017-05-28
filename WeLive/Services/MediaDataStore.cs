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
        public async Task<string> UploadImage(string path,string postID,string index)
        {
            string url = String.Format("api/properties/uploadImage?post_id={0}&image_index={1}", postID, index);
            var requestContent = new MultipartFormDataContent();

            var ImageDate = File.ReadAllBytes(path);
			var imageContent = new ByteArrayContent(ImageDate);
			imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
			requestContent.Add(imageContent, "attachment", Guid.NewGuid().ToString() + ".jpg");
            var response = await client.PostAsync(url , requestContent);
			var attachmentID = await response.Content.ReadAsStringAsync();
            return attachmentID;
        }
    }
}
