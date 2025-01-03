using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Migrations;
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
            };
        }

        public static VerhuurVerzoek ToVerhuurVerzoekFromDto(this VerhuurVerzoekRequestDto verhuurVerzoekRequestDto, string appUserId)
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
                Status = "Pending"
            };
        }

        public static Reservering ToReserveringFromVerhuurVerzoek(VerhuurVerzoek verhuurVerzoek){
            return new Reservering
            {    
                AppUserId = verhuurVerzoek.AppUserId,
                VoertuigId = verhuurVerzoek.VoertuigId,
                StartDatum = verhuurVerzoek.StartDatum,
                EindDatum = verhuurVerzoek.EindDatum,
                Bestemming = verhuurVerzoek.Bestemming,
                VerwachtteKM = verhuurVerzoek.VerwachtteKM,
                AardReis = verhuurVerzoek.AardReis, 
            };
        }
    }
}