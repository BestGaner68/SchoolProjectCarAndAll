using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Voertuig;
using api.Dtos.WagenParkDtos;
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

        public static NieuwWagenParkVerzoek ToNieuwWagenParkVerzoekVanNieuwWagenParkDto (NieuwWagenParkVerzoekDto nieuwWagenParkVerzoekDto)
        {
            return new NieuwWagenParkVerzoek
            {
                Voornaam = nieuwWagenParkVerzoekDto.Voornaam,
                Achternaam = nieuwWagenParkVerzoekDto.Achternaam,
                GewensdeUsername = nieuwWagenParkVerzoekDto.GewensdeUsername,
                Email = nieuwWagenParkVerzoekDto.Email,
                Bedrijfsnaam = nieuwWagenParkVerzoekDto.Bedrijfsnaam,
                KvkNummer = nieuwWagenParkVerzoekDto.KvkNummer
            };

        }
    }

    
}