using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Migrations;
using api.Models;

namespace api.Mapper
{
    public class WagenParkMapper
    {
        public static WagenPark toWagenParkFromRegisterOfficeWorkerDto(RegisterOfficeWorkerDto WagenparkDto)
        {  
            WagenPark CurrentWagenpark = new WagenPark(){
                Bedrijfsnaam = WagenparkDto.Bedrijfsnaam,
                BedrijfsString = WagenparkDto.BedrijfsString
            };
            return CurrentWagenpark;
        }
    }
}