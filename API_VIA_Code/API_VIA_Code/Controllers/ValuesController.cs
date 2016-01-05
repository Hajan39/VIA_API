using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_VIA_Code.Controllers
{
    public class ValuesController : ApiController
    {

        // GET api/values
        public string Get()
        {
            return "Za lomeno vložit uživatelské jméno z GitHubu.";
        }

        // GET api/values/5
        public GetResult Get(string id)
        {
            var result = new GetResult();
            result.UserName = id;
            result.Email = result.getGitEmail(id);
            if (result.Email != null)
            {
                result.EmailHash = result.CalculateMD5Hash(result.Email.ToLower());
                result.Photos = result.getGravatarImages(result.EmailHash);
            }
            result.Repos = result.getRepos(id);
            if (result.Photos == null)
                result.Photos = result.getGravatarImagesUsername(result.UserName);
            else
                result.Photos.Concat(result.getGravatarImagesUsername(result.UserName));

            return result;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}