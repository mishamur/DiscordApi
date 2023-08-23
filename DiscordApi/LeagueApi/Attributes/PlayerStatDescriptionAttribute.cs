using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordApi.LeagueApi.Attributes
{
    internal class PlayerStatDescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public PlayerStatDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
