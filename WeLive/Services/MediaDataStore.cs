using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Collections.Generic;
using Plugin.Connectivity;

namespace WeLive
{
    public class MediaDataStore:BaseDataStore
    {
        public MediaDataStore()
        {
        }
        public async Task<string> UploadImage(string path,string postID,string index)
        {
			if (!CrossConnectivity.Current.IsConnected)
				throw new Exception(ErrorMessage.NotLogin);
            
            string url = String.Format("api/properties/uploadImage?post_id={0}&image_index={1}", postID, index);
            var requestContent = new MultipartFormDataContent();

            var ImageDate = File.ReadAllBytes(path);
			var imageContent = new ByteArrayContent(ImageDate);
			imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            requestContent.Add(imageContent, "attachment", System.IO.Path.GetFileName(path));
            var response = await client.PostAsync(url , requestContent);
			var attchmentUrl = await response.Content.ReadAsStringAsync();
			
            ErrorMessage.CheckRespond(attchmentUrl);
            attchmentUrl = attchmentUrl.Trim();
            attchmentUrl = attchmentUrl.Replace("\\/","/").Replace("\"","");
            return attchmentUrl;
        }

        public async Task<int> GetMaxUploadImageCount()
        {
			if (!CrossConnectivity.Current.IsConnected)
				throw new Exception(ErrorMessage.NotLogin);
            
            string url = "api/properties/get_max_image_count";
            var count = await client.GetStringAsync(url);
            ErrorMessage.CheckRespond(url);
            return System.Int32.Parse(count.Trim().Replace("\"",""));
        }
    }
}
