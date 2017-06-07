using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

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
        public const string ServerReturnWarning = "warning";

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
            string decode = respond.Trim().Replace("\"","");
           
            if (ErrorContainCode(decode,ErrorMessage.NotLogin))
            {
                throw new Exception(ErrorMessage.NotLogin) ;
            }
            else if (ErrorContainCode(decode, ErrorMessage.ServerReturnError))
            {
                throw new Exception(ErrorMessage.ServerReturnError);
            }
            else if (ErrorContainCode(decode, ErrorMessage.ServerReturnWarning))
            {
                throw new Exception(decode = Unicode2String(decode.Substring(ServerReturnWarning.Length)));
            }
        }

		public static string Unicode2String(string source)
		{
			return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
						 source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
		}
      
        public static void ValidateEmpty(string input, string message1, string message2)
        {
            if (string.IsNullOrEmpty(input))
                throw new Exception(string.Format("请输入{0}(please enter {1})", message1, message2)); 
        }
		static Regex ValidEmailRegex = CreateValidEmailRegex();

		private static Regex CreateValidEmailRegex()
		{
			string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
				+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
				+ @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

			return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
		}

		public static void ValidateEmail(string emailAddress)
		{
            if (!ValidEmailRegex.IsMatch(emailAddress))
                throw new Exception("邮件地址不合法(Email address is invalid)");
		}

		public static void ValidateEqual(string var1,string var2, string msg)
		{
            if (var1 != var2)
            {
                throw new Exception(msg); 
            }
		}
    }
}
