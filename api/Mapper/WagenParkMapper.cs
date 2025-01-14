using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Voertuig;
using api.Migrations;
using api.Models;

namespace api.Mapper
{
    public class WagenParkMapper
    {
        public static WagenPark toWagenParkFromRegisterOfficeWorkerDto(RegisterWagenParkBeheerderDto WagenparkDto)
        {  
        WagenPark CurrentWagenpark = new()
            {
            Bedrijfsnaam = WagenparkDto.Bedrijfsnaam,
            BedrijfsString = WagenparkDto.BedrijfsString,
            KvkNummer = WagenparkDto.KvkNummer,
            };
        return CurrentWagenpark;
        }

        public static WagenParkOverzichtDto ToOverzichtDto(Reservering reservering, VoertuigDto voertuigDto, AppUser appUser, string status){
            return new WagenParkOverzichtDto
            {
                StartDatum = reservering.StartDatum,
                EindDatum = reservering.EindDatum,
                AardReis = reservering.AardReis,
                Bestemming = reservering.Bestemming,
                VerwachtteKM = reservering.VerwachtteKM,
                VoertuigMerk = voertuigDto.Merk,
                VoertuigSoort = voertuigDto.Soort,
                VoertuigType = voertuigDto.type,
                VolledigeNaam = $"{appUser.Voornaam} {appUser.Achternaam}",
                Username = appUser.UserName,
                VoertuigStatus = status,
                ReserveringStatus = reservering.Status,
            };
        }
    }

    
}