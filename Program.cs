// Coleman McClelland
// Calance Interview Coding Challenge
// February 11, 2021

using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace ColemanBitbucketApiChallenge
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            dynamic json = DoGet("https://bitbucket.org/api/2.0/repositories/calanceus/interviews17/commits");
            Tuple<DateTime, string>[] timestampedMessages = ExtractTimestampedMessages(json["values"]);
            Array.Sort(timestampedMessages);
            Array.Reverse(timestampedMessages);
            WriteToFile(@".\coleman_bitbucket_output.txt", timestampedMessages);
        }

        static dynamic DoGet(string url)
        {
            string response = client.GetStringAsync("https://bitbucket.org/api/2.0/repositories/calanceus/interviews17/commits").Result;
            return JsonConvert.DeserializeObject(response);
        }

        static Tuple<DateTime, string>[] ExtractTimestampedMessages(dynamic json)
        {
            Tuple<DateTime, string>[] result = new Tuple<DateTime, string>[json.Count];
            for (int index = 0; index < json.Count; ++index)
            {
                result[index] = Tuple.Create(((DateTime)json[index]["date"]).ToUniversalTime(), (string)json[index]["message"]);
            }
            return result;
        }

        static void WriteToFile(string filePath, Tuple<DateTime, string>[] messages)
        {
            StreamWriter outputFile = File.CreateText(filePath);
            using (outputFile)
            {
                for (int i = 0; i < messages.Length; ++i)
                {
                    outputFile.WriteLine(messages[i].Item1.ToString("yyyy-MM-ddTHH:mm:sszzz") + " " + messages[i].Item2);
                }
            }
            outputFile.Close();
        }
    }
}
