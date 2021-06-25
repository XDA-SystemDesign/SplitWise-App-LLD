using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Models
{
    //[JsonObject]
    public class Group
    {
        //[JsonProperty("GroupId")]
        public int GroupId { get; set; }
        //[JsonProperty("Name")]
        public string name { get; set; }
        //[JsonProperty("Users")]
        public List<User> users { get; set; }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine($"Group ID is {GroupId} for name {name}");
            foreach (var user in users)
            {
                s.AppendLine(user.ToString());
            }
            return s.ToString();
        }
    }
}
