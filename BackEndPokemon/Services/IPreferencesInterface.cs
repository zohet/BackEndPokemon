using BackEndPokemon.Models;
using System.Collections.Generic;

namespace BackEndPokemon.Services
{
    public interface IPreferencesInterface
    {
        Response CreateUser(PreferenceUser data);
        Response UserExist(string UserName);
        Response GetPreferences(string UserName);

        Response UpdatePreferences(string UserName, List<int> preferencesList);

        Response AddOnePreference(string UserName, int preference);

        Response DeleteUserPreferencences(string UserName);

    }
}
