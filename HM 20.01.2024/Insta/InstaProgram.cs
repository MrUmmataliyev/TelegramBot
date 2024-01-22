using System.IO.Compression;
using Insta;

namespace Insta
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            const string token = "6555041163:AAESZzhkcd6lVZ78jRckCzNA9ImV_nA1q9I";

            TelegramBotHandler handler = new TelegramBotHandler(token);

            try
            {
                await handler.BotHandle();
            }
            catch (Exception ex)
            {
                throw new Exception("nima gap");
            }

            // string startPath = @"D:\dotnetexternal\flyuchun\test";
            // string zipPath = @"D:\dotnetexternal\flyuchun\result.zip";
            //// string extractPath = @".\extract";

            // ZipFile.CreateFromDirectory(startPath, zipPath);

            //ZipFile.ExtractToDirectory(zipPath, extractPath);

        }
    }
}
