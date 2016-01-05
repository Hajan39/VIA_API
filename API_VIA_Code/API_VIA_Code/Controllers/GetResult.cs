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

            return sb.ToString().ToLower();

        }

        internal string getGitEmail(string id)
        {

            object o = GetResponse(string.Format("https://api.github.com/users/{0}", id));

            return o.ToString();

        }

        internal IEnumerable<string> getRepos(string id)
        {
            var listOfRepos = new List<string>();

            foreach (var user in GetResponse(string.Format("https://api.github.com/users/{0}/repos", id)))
            {
                listOfRepos.Add((string)user.name);
            }


            return listOfRepos;
        }

        internal IEnumerable<string> getGravatarImages(string email)
        {
            var listOfImages = new List<string>();

            foreach (var entry in GetResponse(string.Format("https://www.gravatar.com/{0}.json", email)).entry)
            {
                foreach (var photo in entry.photos)
                {
                    listOfImages.Add((string)photo.value);
                }

            }
            return listOfImages;
        }

        internal IEnumerable<string> getGravatarImagesUsername(string userName)
        {
            string hashMail = (string)GetResponse(string.Format("https://www.gravatar.com/{0}.json", userName)).entry[0].hash;
            return getGravatarImages(hashMail);
        }

        private dynamic GetResponse(string command)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers["User-Agent"] = "MyApp";
                    var response = client.DownloadString(command);
                    return JsonConvert.DeserializeObject(response);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
