using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using UnityEngine;


public class LogUploader
    {
        public string Url { get; set; }
        public long StreamigBlockSize { get; set; }
        public event EventHandler<ProgressEventArgs> Compressing;
        public event EventHandler<ProgressEventArgs> Uploading;
        public event EventHandler<CompletedEventArgs> Completed;
        public event EventHandler<ErrorEventArgs> Error;

        public LogUploader(string url, long streamigBlockSize)
        {
            Url = url;
            StreamigBlockSize = streamigBlockSize;
        }

        private FileInfo Compress(string path)
        {
            CompressionHelper.CompressFile(path);

            //test
            //var test = Application.dataPath + "/" + "output_log_test.txt";
            //CompressionHelper.DecompressFile(test);

            var compressFile = new FileInfo(path);

            return compressFile;
        }


        void StartCompress()
        {
            // Convert 10000 character string to byte array.
            byte[] text1 = Encoding.ASCII.GetBytes(new string('X', 10000));
            byte[] compressed = CompressionHelper.CompressBytes(text1);
            byte[] text2 = CompressionHelper.DecompressBytes(compressed);

            string longstring = "defined input is deluciously delicious.14 And here and Nora called The reversal from ground from here and executed with touch the country road, Nora made of, reliance on, can’t publish the goals of grandeur, said to his book and encouraging an envelope, and enable entry into the chryssial shimmering of hers, so God of information in her hands Spiros sits down the sign of winter? —It’s kind of Spice Christ. It is one hundred birds circle above the text: They did we said. 69 percent dead. Sissy Cogan’s shadow. —Are you x then sings.) I’m 96 percent dead humanoid figure,";
            byte[] text3 = Encoding.ASCII.GetBytes(longstring);
            byte[] compressed2 = CompressionHelper.CompressBytes(text3);
            byte[] text4 = CompressionHelper.DecompressBytes(compressed2);

            Debug.Log("text1 size: " + text1.Length);
            Debug.Log("compressed size:" + compressed.Length);
            Debug.Log("text2 size: " + text2.Length);
            Debug.Log("are equal: " + ByteArraysEqual(text1, text2));

            Debug.Log("text3 size: " + text3.Length);
            Debug.Log("compressed2 size:" + compressed2.Length);
            Debug.Log("text4 size: " + text4.Length);
            Debug.Log("are equal: " + ByteArraysEqual(text3, text4));
        }

        public bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }





        public void UploadLog(string fileName)
        {
            FileInfo file = Compress(fileName);

            HttpWebRequest request = null;
            Stream requestStream = null;
            FileStream fileStream = null;

            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(Url + "?name=" + file.Name + "&chunkSize=" + StreamigBlockSize);
                request.Method = "POST";
                request.ContentType = "multipart/form-data";
                request.SendChunked = true;
                request.AllowWriteStreamBuffering = false;
                request.KeepAlive = true;
                request.Timeout = 600000;

                requestStream = request.GetRequestStream();
                fileStream = new FileStream(file.FullName, FileMode.Open);

                byte[] buffer = new byte[StreamigBlockSize];
                int read;
                long progressCount = 0;
                while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    requestStream.Write(buffer, 0, read);

                    progressCount += read;
                    long percentage = progressCount * 100 / file.Length;
                    OnUploading(percentage);
                }

                requestStream.Close();
                fileStream.Close();

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    response.Close();
                    File.Delete(file.FullName);
                    OnCompleted();
                }
                catch (WebException ex)
                {
                    StreamReader sReader = new StreamReader(ex.Response.GetResponseStream());
                    string errorMsg = sReader.ReadToEnd().Replace("\"", "");
                    sReader.Close();

                    if (fileStream != null) { fileStream.Dispose(); }
                    if (requestStream != null) { requestStream.Dispose(); }
                    if (request != null) { request.Abort(); }

                    File.Delete(file.FullName);
                    OnError(errorMsg);
                }
            }
            catch (Exception ex)
            {
                if (fileStream != null) { fileStream.Dispose(); }
                if (requestStream != null) { requestStream.Dispose(); }
                if (request != null) { request.Abort(); }

                File.Delete(file.FullName);
                OnError(ex.Message);
            }
        }



        private void OnCompressing(long percentage)
        {
            if (Compressing != null) { Compressing(this, new ProgressEventArgs(percentage)); }
        }

        private void OnUploading(long percentage)
        {
            if (Uploading != null) { Uploading(this, new ProgressEventArgs(percentage)); }
        }

        private void OnCompleted(CompletedEventArgs e = null)
        {
            if (Completed != null) { Completed(this, e); }
        }

        private void OnError(string errorMessage)
        {
            if (Error != null) { Error(this, new ErrorEventArgs(errorMessage)); }
        }


        //
        static LogUploader log = new LogUploader("http://52.38.157.199:80/logs/upload", 10000);
        public static void UploadDirectToAWSCarlos(string nameFile)
        {
            log.Completed += log_Completed;
            log.UploadLog(nameFile);
        }

        static void log_Completed(object sender, CompletedEventArgs e)
        {
            MonoBehaviour.print("log uploaded completed");
        }
    }


    public class ProgressEventArgs : EventArgs
    {
        public long Percentage { get; private set; }

        public ProgressEventArgs(long percentage)
        {
            Percentage = percentage;
        }
    }

    public class CompletedEventArgs : EventArgs
    {
        public object[] Args { get; private set; }

        public CompletedEventArgs(object[] args = null)
        {
            Args = args != null ? args : new object[0];
        }
    }

    public class ErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; private set; }

        public ErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
