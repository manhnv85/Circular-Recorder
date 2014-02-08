using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Live;

namespace CircularRecorderApp
{
    public class Utilities
    {
        public static MainPage MainPageControl { get; set; }

        public static MemoryStream MemoryStream
        {
            get;
            set;
        }

        public static string RecordingToUpload
        {
            get;
            set;
        }

        public static LiveConnectSession SkyDriveSession
        {
            get;
            set;
        }

        public static bool IsValidEmailAddress(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.Match(email).Success;
        }
    }
}
