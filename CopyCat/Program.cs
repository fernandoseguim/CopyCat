using System;
using System.IO;
using System.Threading.Tasks;

namespace CopyCat
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            const string BASE_ADDRESS = "BASE_ADDRESS";
            const string USERNAME = "USERNAME";
            const string PERSONAL_ACCESS_TOKEN = "PERSONAL_ACCESS_TOKEN";
            const string PROJECT_NAME = "PROJECT_NAME";

            try
            {
                var azureDevOpsApiClient = new AzureDevOpsApiClient();

                var repositories = await azureDevOpsApiClient.GetRepositoriesAsync(BASE_ADDRESS, PROJECT_NAME, PERSONAL_ACCESS_TOKEN);

                var path = $"D:\\workspace\\{PROJECT_NAME}";
                Directory.CreateDirectory(path);

                foreach(var repository in repositories)
                {
                    var remoteUrl = repository.RemoteUrl;
                    var organization = remoteUrl.Substring(0, remoteUrl.IndexOf('@'));
                    var uri = remoteUrl.Replace($"{organization}", $"https://{USERNAME}:{PERSONAL_ACCESS_TOKEN}");

                    LibGit2Sharp.Repository.Clone(uri, Path.Combine(path, repository.Name));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
