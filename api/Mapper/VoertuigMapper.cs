using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.VoertuigDtos;

using api.Models;

namespace api.Mapper
{
    public class VoertuigMapper
    {
        public static Voertuig FromNieuweVoertuigDtoToVoertuig(NieuwVoertuigDto nieuwVoertuigDto)
        {
            return new Voertuig
            {
                Merk = nieuwVoertuigDto.Merk,
                Kenteken = nieuwVoertuigDto.Kenteken,
                Kleur = nieuwVoertuigDto.Kleur,
                Type = nieuwVoertuigDto.Type,
                AanschafJaar = nieuwVoertuigDto.AanschafJaar,
                Soort = nieuwVoertuigDto.Soort
            };
        }

        public static void MapWeizigVoertuigDtoToVoertuig(WeizigVoertuigDto weizigVoertuigDto, Voertuig currentVoertuig)
        {
            if (!string.IsNullOrEmpty(weizigVoertuigDto.Merk))
                currentVoertuig.Merk = weizigVoertuigDto.Merk;

            if (!string.IsNullOrEmpty(weizigVoertuigDto.Kenteken))
                currentVoertuig.Kenteken = weizigVoertuigDto.Kenteken;

            if (!string.IsNullOrEmpty(weizigVoertuigDto.Kleur))
                currentVoertuig.Kleur = weizigVoertuigDto.Kleur;

            if (!string.IsNullOrEmpty(weizigVoertuigDto.Type))
                currentVoertuig.Type = weizigVoertuigDto.Type;

            if (weizigVoertuigDto.AanschafJaar.HasValue)
                currentVoertuig.AanschafJaar = weizigVoertuigDto.AanschafJaar.Value;

            if (!string.IsNullOrEmpty(weizigVoertuigDto.Soort))
                currentVoertuig.Soort = weizigVoertuigDto.Soort;
        }

    }
}