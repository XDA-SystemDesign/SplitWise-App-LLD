using Newtonsoft.Json;
using System;

namespace SplitWise_Models
{
    //[JsonObject("User")]
    //[Serializable]
    public class User
    {
        //[JsonProperty("UserID")]
        public int Userid { get; set; }
        //[JsonProperty("UserName")]
        public string name { get; set; }
        public User(int userId, string name)
        {
            this.Userid = userId;
            this.name = name;
        }
        public override string ToString()
        {
            return $"{name} - {Userid}";
        }
    }
}
