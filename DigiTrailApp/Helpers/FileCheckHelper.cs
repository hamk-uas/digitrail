using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DigiTrailApp.Helpers
{
    class FileCheckHelper
    {
        /// <summary>
        /// Check when file is created and compare it hours value
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="hours">negative hours</param>
        /// <returns>true if file is older than added negative hours </returns>
        public bool IsFileOlderThan(string path, double hours)
        {
            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                if (info.CreationTimeUtc < DateTime.UtcNow.AddHours(hours))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;

        }
    }
}