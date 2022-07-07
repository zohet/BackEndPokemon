﻿namespace BackEndPokemon.Models
{
    public class Response
    {
        public int Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public Response()
        {
            this.Success = 0;
        }
    }
}
