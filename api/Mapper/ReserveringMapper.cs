using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.ReserveringenEnSchade;
using api.Dtos.Verhuur;
using api.Dtos.VoertuigDtos;
using api.Models;

namespace api.Mapper
{
    public class ReserveringMapper
    {
        public static HuurGeschiedenisDto ToHuurGeschiedenisDto(Reservering reservering, VoertuigDto voertuigDto){
            return new HuurGeschiedenisDto
            {
                StartDatum = reservering.StartDatum,
                EindDatum = reservering.EindDatum,
                AardReis = reservering.AardReis,
                Bestemming = reservering.Bestemming,
                VerwachtteKM = reservering.VerwachtteKM,
                VoertuigMerk = voertuigDto.Merk,
                VoertuigSoort = voertuigDto.Soort,
                VoertuigType = voertuigDto.type,
                Accessoires = reservering.Accessoires,
                Verzekering = reservering.Verzekering.VerzekeringNaam,
            };
        }
    }
}