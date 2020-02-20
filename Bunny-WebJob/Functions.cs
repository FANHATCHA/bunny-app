using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;
using NReco.VideoConverter;


namespace Bunny_WebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void GenerateVideoSample(
            [QueueTrigger("bunnymaker")] string message,
            [Blob("videosplaylist/videos/{queueTrigger}")] CloudBlockBlob inputBlob,
            [Blob("videosplaylist/samples/{queueTrigger}")] CloudBlockBlob outputBlob,
            TextWriter log)
        {
            log.WriteLine(message);
          
            // Open streams to blobs for reading and writing as appropriate.
            // Pass references to application specific methods
            using (Stream input = inputBlob.OpenRead())
            using (Stream output = outputBlob.OpenWrite())
            {
                createSample(input, output, 20);
                outputBlob.Properties.ContentType = "video/mp4";

                // Set title attribute
                inputBlob.FetchAttributes();
                outputBlob.Metadata["Title"] = inputBlob.Metadata["Title"];
                //outputBlob.SetMetadata();

            }
            log.WriteLine("GenerateVideoSample() completed...");
        }

        private static void createSample(Stream input, Stream output, int duration)
        {

            /* Todo
             *  Limit video files to mp4s and to a max length of 20MB.
             **/

            BinaryWriter Writer = null;
            try
            {
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.Open("temp.mp4", FileMode.Create));
                BinaryReader Reader = new BinaryReader(input);
                byte[] imageBytes = null;
                imageBytes = Reader.ReadBytes((int)input.Length);
                // Writer raw data                
                Writer.Write(imageBytes);
                Writer.Flush();
                Writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("*** FileWrite exception: " + e.Message);

            }

            var vid_duration = new ConvertSettings();
            vid_duration.MaxDuration = duration;

            var ffMpeg = new FFMpegConverter();
            ffMpeg.ConvertMedia("temp.mp4", "mp4", output, "mp4", vid_duration);

        }
    }
}
