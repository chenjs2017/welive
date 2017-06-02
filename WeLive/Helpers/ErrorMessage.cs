using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace WeLive
{
    public class ErrorMessage
    {
        public ErrorMessage()
        {
        }
		public const string NetworkIssue = "network";
		public const string LoginFail = "logfail";
        public const string NotLogin = "nologin";
        public const string ServerReturnError = "error";
        public const string DecodeEorror = "decodeerror";

        public static string GetMessage(string messageCode)
        {
            if (messageCode== NetworkIssue)
            {
                return "网络连接失败，请稍后再试(Network issue, please try again later)";
            }
            else if (messageCode == LoginFail)
            {
                return "登录失败，请检查用户名和密码(Login Fail, please check username and password)";
            } 
            else if (messageCode == NotLogin)
            {
                return "请重新登录(Please login)";
            } 
            else if (messageCode == ServerReturnError)
            {
                return "服务器错误，请稍后再试（Server Error, Please try again later）";
            }
            else if (messageCode == DecodeEorror)
            {
				return "解析服务器信息失败，请稍后再试（Decode Error, Please try again later）";
			}
            return messageCode;
        }

        public static bool ErrorContainCode(string respond, string err)
        {
            return respond.Trim().Replace("\"", "").StartsWith(err,StringComparison.CurrentCulture); 
        }
        public static void CheckRespond(string respond)
        {
            if (ErrorContainCode(respond,ErrorMessage.NotLogin))
            {
                throw new Exception(ErrorMessage.NotLogin) ;
            }
            else if (ErrorContainCode(respond,ErrorMessage.ServerReturnError))
            {
                throw new Exception(ErrorMessage.ServerReturnError);
            }
        }

    }
}
