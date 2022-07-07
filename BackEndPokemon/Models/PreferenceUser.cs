using MongoDB.Bson;
using System.Collections.Generic;

namespace BackEndPokemon.Models
{
    public class PreferenceUser
    {
        public ObjectId _id { get; set; }

        public string UserName { get; set; }

        public List<int> PreferenceList { get; set; }

        public PreferenceUser(string username, List<int> preferencelist)
        {
            this.UserName = username;
            this.PreferenceList = preferencelist;
        }
    }

    public class AddOnePreference
    {
        public string UserName { get; set; }

        public int Preference { get; set; }
    }

    public class UpdatePreferences
    {
        public string UserName { get; set; }

        public List<int> PreferenceList { get; set; }
    }
}
