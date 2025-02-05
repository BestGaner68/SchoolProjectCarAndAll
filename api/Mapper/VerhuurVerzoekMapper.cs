using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DataStructureClasses;
using api.Dtos.Account;
using api.Dtos.Verhuur;
using api.Dtos.VoertuigDtos;
using api.Interfaces;
using api.Models;

namespace api.Mapper
{
    public static class VerhuurVerzoekMapper
    {
        public static VerhuurVerzoekDto ToVerhuurVerzoekDto(this VerhuurVerzoek verhuurVerzoek)
        {
            return new VerhuurVerzoekDto
            {
                StartDatum = verhuurVerzoek.StartDatum,
                EindDatum = verhuurVerzoek.EindDatum,
                AardReis = verhuurVerzoek.AardReis,
                Bestemming = verhuurVerzoek.Bestemming,
                VerwachtteKM = verhuurVerzoek.VerwachtteKM,
                Datum = verhuurVerzoek.Datum,
                VerhuurVerzoekId = verhuurVerzoek.VerhuurVerzoekId,
            };
            
        }

        public static VerhuurVerzoek ToVerhuurVerzoekFromDto(
        this VerhuurVerzoekRequestDto verhuurVerzoekRequestDto,
        string appUserId,
        List<Accessoires> gekozenAccessoires,
        Verzekering verzekering) 
        {
            return new VerhuurVerzoek
            {
                AppUserId = appUserId,
                VoertuigId = verhuurVerzoekRequestDto.VoertuigId,
                StartDatum = verhuurVerzoekRequestDto.StartDatum,
                EindDatum = verhuurVerzoekRequestDto.EindDatum,
                Bestemming = verhuurVerzoekRequestDto.Bestemming,
                VerwachtteKM = verhuurVerzoekRequestDto.VerwachtteKM,
                AardReis = verhuurVerzoekRequestDto.AardReis,
                Datum = DateTime.Now,
                Status = VerhuurVerzoekStatussen.Pending,
                Accessoires = gekozenAccessoires,
                Verzekering = verzekering,
            };
        }

        public static Reservering ToReserveringFromVerhuurVerzoek(
        VerhuurVerzoek verhuurVerzoek)
        {
            return new Reservering
            {    
                AppUserId = verhuurVerzoek.AppUserId,
                VoertuigId = verhuurVerzoek.VoertuigId,
                StartDatum = verhuurVerzoek.StartDatum,
                EindDatum = verhuurVerzoek.EindDatum,
                Bestemming = verhuurVerzoek.Bestemming,
                VerwachtteKM = verhuurVerzoek.VerwachtteKM,
                AardReis = verhuurVerzoek.AardReis,
                Accessoires = verhuurVerzoek.Accessoires,
                Verzekering = verhuurVerzoek.Verzekering,
            };
        }

        public static VolledigeDataDto ToVolledigeDataDto(VerhuurVerzoek verhuurVerzoek, string Fullname, VoertuigDto voertuigDto){
            return new VolledigeDataDto
            {
                StartDatum = verhuurVerzoek.StartDatum,
                EindDatum = verhuurVerzoek.EindDatum,
                AardReis = verhuurVerzoek.AardReis,
                Bestemming = verhuurVerzoek.Bestemming,
                VerwachtteKM = verhuurVerzoek.VerwachtteKM,
                VolledigeNaam = Fullname,
                VoertuigMerk = voertuigDto.Merk,
                VoertuigSoort = voertuigDto.Soort,
                VoertuigType = voertuigDto.type,
                VerhuurverzoekId = verhuurVerzoek.VerhuurVerzoekId
            };
        }
    }
}