using Android.App;
using Android.Util;
using DigiTrailApp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DigiTrailApp.AsyncTasks
{
    class DownloadTask
    {
        public static CancellationTokenSource TokenSource { private set; get; }
        public static bool IsTaskRunning { private set; get; }

        public DownloadTask(CancellationTokenSource tokenSource)
        {
            TokenSource = tokenSource;
        }

        /// <summary>
        /// Starts a new background task to fetch all GPX files from remote location and stores them to device's storage
        /// </summary>
        public void DownloadGPXFiles(List<string> remoteLocations)
        {
            CancellationToken token = TokenSource.Token;

            Task.Run(() =>
            {
                IsTaskRunning = true;

                try
                {
                    //For debug purposes, remove before release (still in here #life)
                    int i = 0;

                    foreach (string remotePath in remoteLocations)
                    {
                        token.ThrowIfCancellationRequested();

                        // Start figuring out the filename of the GPX file
                        int pos = remotePath.LastIndexOf("/");
                        // Define the path of the file in the filesystem
                        string path = Application.Context.FilesDir.AbsolutePath + remotePath.Substring(pos, remotePath.Length - pos);
                        FileCheckHelper fch = new FileCheckHelper();
                        if (fch.IsFileOlderThan(path,-1))
                        {
                            // Check if we have an active internet connection
                            if (ServiceInformation.IsConnectionActive())
                            {
                                // Fetch the GPX file from Internet
                                string content = new HttpRequest().GetString(remotePath).Result;

                                // Check if the content is not null or empty
                                if (!string.IsNullOrEmpty(content))
                                {
                                    // Write the GPX file into the memory of the device
                                    File.WriteAllText(path, content);
                                }
                            }
                        }

                        i++;
                    }
                }
                catch (OperationCanceledException)
                {
                    
                }
                finally
                {
                    TokenSource.Dispose();
                    IsTaskRunning = false;
                }

            }, token);
        }       
    }
}
