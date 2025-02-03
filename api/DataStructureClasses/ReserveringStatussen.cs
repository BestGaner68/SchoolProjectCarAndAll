using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DataStructureClasses
{
    /// <summary>
    /// namen van de reserveringen voor consistentie
    /// </summary>
    public class ReserveringStatussen
    {
        public const string MagWordenGewijzigd = "MagWordenGewijzigd";
        public const string ReadyForPickUp = "ReadyForPickUp";
        public const string Geweigerd = "Geweigerd";
        public const string Uitgegeven = "Uitgegeven";
        public const string Afgerond = "Afgerond";
    }
}