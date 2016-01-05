using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace API_VIA_Code.Controllers
{
    public class GetResult
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string EmailHash { get; set; }

        public IEnumerable<string> Repos { get; set; }

        public IEnumerable<string> Photos { get; set; }

        public string CalculateMD5Hash(string input)
        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);



            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {

                sb.Append(hash[i].ToString("X2"));

            }

            return sb.ToString();

        }

        internal string getGitEmail(string id)
        {
            using (var client = new WebClient())
            {

                client.Headers["User-Agent"] = "MyApp";

                var response = client.DownloadString(string.Format("https://api.github.com/users/{0}", id));

                dynamic res = JsonConvert.DeserializeObject(response);

                return res.email;
            }
        }

        internal IEnumerable<string> getRepos(string id)
        {
            var listOfRepos = new List<string>();
            using (var client = new WebClient())
            {

                client.Headers["User-Agent"] = "MyApp";

                var response = client.DownloadString(string.Format("https://api.github.com/users/{0}/repos", id));

                dynamic res = JsonConvert.DeserializeObject(response);
                foreach (var user in res)
                {
                    listOfRepos.Add((string)user.name);
                }

            }
            return listOfRepos;
        }

        internal IEnumerable<string> getGravatarImages(string email)
        {
            var listOfImages = new List<string>();
            using (var client = new WebClient())
            {

                client.Headers["User-Agent"] = "MyApp";

                var response = client.DownloadString(string.Format("https://www.gravatar.com/{0}.json", email));

                dynamic res = JsonConvert.DeserializeObject(response);

                foreach (var entry in res.entry)
                {
                    foreach (var photo in entry.photos)
                    {
                        listOfImages.Add((string)photo.value);
                    }
                }
            }
            return listOfImages;
        }
    }
}
