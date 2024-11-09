using System;
using Orquesta.Shared.Entities;

namespace Orquesta.Shared.DTOs
{
    public class TokenDTO
    {
        public string Token { get; set; } = null!;

        public DateTime Expiration { get; set; }
    }
}

