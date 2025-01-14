using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IAbonnementService
    {
        public Task<List<Abonnement>> getAllAbonnementen (); //methode returned alle abonnementen uit de abonnementen Db tabel
        public Task<bool> ChangeAbonnement (int AbonnementId);//Methode wordt door WagenParkbeheerder gebruikt om hun abbonement aan te passen
    }
}