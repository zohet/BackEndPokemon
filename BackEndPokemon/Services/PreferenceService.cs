using BackEndPokemon.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackEndPokemon.Services
{
    public class PreferenceService : IPreferencesInterface
    {
        private readonly ConectionData _appSettings;
        
        public PreferenceService(IOptions<ConectionData> conection)
        {
            _appSettings = conection.Value;
        }
        public Response CreateUser(PreferenceUser data)
        {
            Response newRes = new Response();

            var preferenceUserCollection = GetCollection();
            preferenceUserCollection.InsertOne(data);
            
            newRes.Success = 1;
            newRes.Message = "Se crea usuario";
            newRes.Data = GenerateToken(data.UserName);

            return newRes;
        }

        public Response AddOnePreference(string UserName, int preference)
        {
            Response newRes = new Response();

            var preferenceUserCollection = GetCollection();
            
            //Filter info

            FilterDefinition<PreferenceUser> filter = Builders<PreferenceUser>.Filter.Eq("UserName", UserName);

            //One

            UpdateDefinition<PreferenceUser> update = Builders<PreferenceUser>.Update.AddToSet<int>("PreferenceList", preference);

            preferenceUserCollection.UpdateOne(filter, update);

            newRes.Success = 1;
            newRes.Message = "Se agrega preferencia";

            return newRes;
        }

        public Response UpdatePreferences(string UserName, List<int> preferencesList)
        {
            Response newRes = new Response();

            var preferenceUserCollection = GetCollection();

            //Filter info

            FilterDefinition<PreferenceUser> filter = Builders<PreferenceUser>.Filter.Eq("UserName", UserName);

            //Multiple

            UpdateDefinition<PreferenceUser> update = Builders<PreferenceUser>.Update
                .Set("PreferenceList", preferencesList);

            preferenceUserCollection.UpdateOne(filter, update);

            newRes.Success = 1;
            newRes.Message = "Se borra preferencia";

            return newRes;
        }

        public Response DeleteUserPreferencences(string UserName)
        {
            var preferenceUserCollection = GetCollection();

            //Filter info

           FilterDefinition<PreferenceUser> filter = Builders<PreferenceUser>.Filter.Eq("UserName", UserName);

            preferenceUserCollection.DeleteOne(filter);

            Response newRes = new Response();

            newRes.Success = 1;
            newRes.Message = "Se borran los datos del usuario";

            return newRes;
        }

        public Response UserExist(string UserName)
        {
            var preferenceUserCollection = GetCollection();
            Response newRes = new Response();
            //Filter info

            FilterDefinition<PreferenceUser> filter = Builders<PreferenceUser>.Filter.Eq("UserName", UserName);

            var user = preferenceUserCollection.Find(filter).FirstOrDefault();

            if(user != null)
            {
                
                newRes.Success = 2;
                newRes.Message = $"Hola de nuevo {user.UserName}";
                newRes.Data = GenerateToken(UserName);
            }

            return newRes;
        }

        public Response GetPreferences(string UserName)
        {
            var preferenceUserCollection = GetCollection();
            Response newRes = new Response();
            //Filter info

            FilterDefinition<PreferenceUser> filter = Builders<PreferenceUser>.Filter.Eq("UserName", UserName);

            var user = preferenceUserCollection.Find(filter).FirstOrDefault();

            if (user != null)
            {

                newRes.Success = 1;
                newRes.Message = "Trayendo preferencias del usuario";
                newRes.Data = user.PreferenceList;
            }

            return newRes;
        }

        private IMongoCollection<PreferenceUser> GetCollection()
        {
            MongoClient client = new MongoClient(_appSettings.KeyMongoDBConnection);
            return client.GetDatabase("PokeDex").GetCollection<PreferenceUser>("PreferenceUser");
        }
        private string GenerateToken(string usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var llave = Encoding.ASCII.GetBytes(_appSettings.KeyJWT);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Email, usuario)
                        }
                    ),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
