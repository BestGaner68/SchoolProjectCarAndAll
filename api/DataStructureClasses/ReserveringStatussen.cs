using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DataStructureClasses
{
    public class ReserveringStatussen
    {
        public const string Pending = "Pending";
        public const string MagWordenGewijzigd = "MagWordenGewijzigd";
         public const string ReadyForPickUp = "ReadyForPickUp";
        public const string Geweigerd = "Geweigerd";
        public const string Geaccepteerd = "Geaccepteerd";
        public const string DoorGebruikerGeweigerd = "DoorGebruikerGeweigerd";
        public const string Uitgegeven = "Uitgegeven";
        public const string Afgerond = "Afgerond";
    }
}