using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterImageArchiver
{
    class Program
    {

        static string SEARCH_PARAMETER = "yfrog.com";
        static string IMAGES_FOLDER = "Images\\";
        static string RESUME_POINTER = "RESUME";
        static string REPLACE_PARAMETER = "http://twitter.yfrog.com";
        static string STARTUP_PATH = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\";

        static string resumePoint = "-0";
        static bool startDownloadsImmediately = true;

        static void Main(string[] args)
        {
            
            string tweetsArchiveFile = STARTUP_PATH + "tweets.csv";
            string[] tweetRows = System.IO.File.ReadAllLines(tweetsArchiveFile);
            
            if (!System.IO.Directory.Exists(STARTUP_PATH + IMAGES_FOLDER))
            {
                System.IO.Directory.CreateDirectory(STARTUP_PATH + IMAGES_FOLDER);
            }

            if (System.IO.File.Exists(STARTUP_PATH + RESUME_POINTER))
            {
                resumePoint = System.IO.File.ReadAllText(STARTUP_PATH + RESUME_POINTER);
                startDownloadsImmediately = false;
            }
            
            foreach (string tweetRow in tweetRows) 
            {
                if (tweetRow.Contains(SEARCH_PARAMETER))
                {
                    Console.WriteLine(GetTweetId(tweetRow));
                    Console.WriteLine(GetYfrogImageUrl(tweetRow));
                    Console.WriteLine(GetYfrogImageDownloadUrl(GetYfrogImageUrl(tweetRow)));

                    if (!startDownloadsImmediately) 
                    {
                        if (GetTweetId(tweetRow) == resumePoint)
                        {
                            startDownloadsImmediately = true;
                        }
                        else 
                        {
                            continue;
                        }
                    }

                    System.Net.WebClient wc = new System.Net.WebClient();

                    // assumes everything is jpeg!
                    string fileToSave = STARTUP_PATH + IMAGES_FOLDER + GetTweetId(tweetRow);

                    try
                    {
                        wc.DownloadFile(GetYfrogImageDownloadUrl(GetYfrogImageUrl(tweetRow)), fileToSave + ".jpg");
                    }
                    catch (Exception ex) 
                    {
                        System.IO.File.WriteAllText(fileToSave + ".txt", "Exception: " + ex.Message);
                    }


                    System.IO.File.WriteAllText(STARTUP_PATH + RESUME_POINTER, GetTweetId(tweetRow));
                    Console.WriteLine("Done: " + GetTweetId(tweetRow));
                    Console.WriteLine("");
                    Console.WriteLine("");
                    
                }
            }

            Console.WriteLine("----");
            Console.WriteLine("Finished all downloads!");
            Console.WriteLine("----");
            Console.Read();

            
        }

        static string GetTweetId(string tweetRow) 
        {
            char[] splitChars = {'\"'};
            return tweetRow.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        static string GetYfrogImageUrl(string tweetRow) 
        {
            string[] splitString = { SEARCH_PARAMETER };
            string[] messyOutput = tweetRow.Split(splitString, StringSplitOptions.RemoveEmptyEntries);

            return REPLACE_PARAMETER + messyOutput[messyOutput.Length - 1].Split('\"')[0].Split(' ')[0];
        }

        static string GetYfrogImageDownloadUrl(string uglyUrl) 
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            string scrapePage = wc.DownloadString(uglyUrl);

            string[] scrapePageTmp = scrapePage.Split(new string[] {"og:image\"" }, StringSplitOptions.None);

            scrapePage = scrapePageTmp[1];

            scrapePageTmp = scrapePage.Split(new string[] { "/>" }, StringSplitOptions.None);

            scrapePageTmp = scrapePageTmp[0].Split('\"');

            return scrapePageTmp[1];
        }
    }
}
