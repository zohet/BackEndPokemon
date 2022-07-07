using BackEndPokemon.Models;
using BackEndPokemon.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BackEndPokemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    public class PokeDexController : ControllerBase
    {
        public IPreferencesInterface _preferences;
        public PokeDexController(IPreferencesInterface preferences)
        {
            _preferences = preferences;
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("CreateUser/{UserName}")]
        public IActionResult CreateUser(string UserName)
        {
            Response newRes = _preferences.UserExist(UserName);
            
            if(newRes.Success == 0)
            {
                List<int> preferencePokemons = new List<int>();
                newRes = _preferences.CreateUser(new PreferenceUser(UserName, preferencePokemons));
            }

            return Ok(newRes);
        }

        [HttpGet]
        [Route("GetPreferences/{UserName}")]
        public IActionResult GetPreferences(string UserName)
        {
            Response newRes = _preferences.GetPreferences(UserName);

            return Ok(newRes);
        }

        [HttpPost("UpdatePreferences")]
        public IActionResult UpdatePreferences(UpdatePreferences data)
        {
            Response newRes = _preferences.UpdatePreferences(data.UserName,data.PreferenceList);

            return Ok(newRes);
        }
        
        [HttpPost("AddOnePreference")]
        public IActionResult AddOnePreference(AddOnePreference data)
        {
            Response newRes = _preferences.AddOnePreference(data.UserName,data.Preference);

            return Ok(newRes);
        }

        [HttpGet]
        [Route("DeleteUserPreferencences/{UserName}")]
        public IActionResult DeleteUserPreferencences(string UserName)
        {
            Response newRes = _preferences.DeleteUserPreferencences(UserName);

            return Ok(newRes);
        }
    }
}
