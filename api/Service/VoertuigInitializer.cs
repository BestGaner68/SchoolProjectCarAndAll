using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DataStructureClasses;
using api.Dtos.Voertuig;
using api.Interfaces;
using api.Models;

namespace api.Service
{
    public static class VoertuigInitializer
    {
        public static async Task InitializeVoertuigenAsync(IVoertuigService voertuigService)
        {
            var bestaandeVoertuigen = await voertuigService.GetAllVoertuigen();
            var bestaandeVoertuigData = await voertuigService.AreAnyVoertuigStatus();
            if (bestaandeVoertuigen.Count != 0 || bestaandeVoertuigData)
            {
                return;
            }     

            var voertuigDtos = new List<InitializeVoertuigDto>
            {
            new() { Merk = "Toyota", Soort = "auto", Kenteken = "AB-123-CD", Kleur = "Rood", AanschafJaar = 2018, Type = "Corolla" },
            new() { Merk = "Ford", Soort = "auto", Kenteken = "EF-456-GH", Kleur = "Blauw", AanschafJaar = 2019, Type = "Focus" },
            new() { Merk = "Volkswagen", Soort = "auto", Kenteken = "IJ-789-KL", Kleur = "Zwart", AanschafJaar = 2020, Type = "Golf" },
            new() { Merk = "Honda", Soort = "auto", Kenteken = "MN-012-OP", Kleur = "Wit", AanschafJaar = 2017, Type = "Civic" },
            new() { Merk = "BMW", Soort = "auto", Kenteken = "QR-345-ST", Kleur = "Grijs", AanschafJaar = 2021, Type = "3 Serie" },
            new() { Merk = "Audi", Soort = "auto", Kenteken = "UV-678-WX", Kleur = "Zilver", AanschafJaar = 2016, Type = "A4" },
            new() { Merk = "Mercedes", Soort = "auto", Kenteken = "YZ-901-AB", Kleur = "Blauw", AanschafJaar = 2022, Type = "C-Klasse" },
            new() { Merk = "Nissan", Soort = "auto", Kenteken = "CD-234-EF", Kleur = "Groen", AanschafJaar = 2015, Type = "Qashqai" },
            new() { Merk = "Peugeot", Soort = "auto", Kenteken = "GH-567-IJ", Kleur = "Rood", AanschafJaar = 2021, Type = "208" },
            new() { Merk = "Renault", Soort = "auto", Kenteken = "KL-890-MN", Kleur = "Zwart", AanschafJaar = 2018, Type = "Clio" },
            new() { Merk = "Hobby", Soort = "auto", Kenteken = "OP-123-QR", Kleur = "Wit", AanschafJaar = 2017, Type = "De Luxe" },
            new() { Merk = "Fendt", Soort = "auto", Kenteken = "ST-456-UV", Kleur = "Grijs", AanschafJaar = 2018, Type = "Bianco" },
            new() { Merk = "Knaus", Soort = "auto", Kenteken = "WX-789-YZ", Kleur = "Blauw", AanschafJaar = 2019, Type = "Sport" },
            new() { Merk = "Dethleffs", Soort = "auto", Kenteken = "AB-012-CD", Kleur = "Groen", AanschafJaar = 2016, Type = "Camper" },
            new() { Merk = "Adria", Soort = "auto", Kenteken = "EF-345-GH", Kleur = "Zilver", AanschafJaar = 2020, Type = "Altea" },
            new() { Merk = "Eriba", Soort = "auto", Kenteken = "IJ-678-KL", Kleur = "Rood", AanschafJaar = 2015, Type = "Touring" },
            new() { Merk = "Tabbert", Soort = "auto", Kenteken = "MN-901-OP", Kleur = "Zwart", AanschafJaar = 2021, Type = "Puccini" },
            new() { Merk = "Burstner", Soort = "auto", Kenteken = "QR-234-ST", Kleur = "Wit", AanschafJaar = 2019, Type = "Premio" },
            new() { Merk = "LMC", Soort = "auto", Kenteken = "UV-567-WX", Kleur = "Blauw", AanschafJaar = 2018, Type = "Musica" },
            new() { Merk = "Sprite", Soort = "auto", Kenteken = "YZ-890-AB", Kleur = "Grijs", AanschafJaar = 2022, Type = "Cruzer" },
            new() { Merk = "Volkswagen", Soort = "auto", Kenteken = "CD-123-EF", Kleur = "Rood", AanschafJaar = 2018, Type = "California" },
            new() { Merk = "Mercedes", Soort = "auto", Kenteken = "GH-456-IJ", Kleur = "Blauw", AanschafJaar = 2019, Type = "Marco Polo" },
            new() { Merk = "Ford", Soort = "auto", Kenteken = "KL-789-MN", Kleur = "Zwart", AanschafJaar = 2020, Type = "Nugget" },
            new() { Merk = "Fiat", Soort = "auto", Kenteken = "OP-012-QR", Kleur = "Wit", AanschafJaar = 2017, Type = "Ducato" },
            new() { Merk = "Citroen", Soort = "auto", Kenteken = "ST-345-UV", Kleur = "Grijs", AanschafJaar = 2021, Type = "Jumper" },
            new() { Merk = "Peugeot", Soort = "auto", Kenteken = "WX-678-YZ", Kleur = "Zilver", AanschafJaar = 2016, Type = "Boxer" },
            new() { Merk = "Renault", Soort = "auto", Kenteken = "AB-901-CD", Kleur = "Blauw", AanschafJaar = 2022, Type = "Master" },
            new() { Merk = "Iveco", Soort = "auto", Kenteken = "EF-234-GH", Kleur = "Groen", AanschafJaar = 2015, Type = "Daily" },
            new() { Merk = "Opel", Soort = "auto", Kenteken = "IJ-567-KL", Kleur = "Rood", AanschafJaar = 2021, Type = "Movano" },
            new() { Merk = "Nissan", Soort = "auto", Kenteken = "MN-890-OP", Kleur = "Zwart", AanschafJaar = 2018, Type = "NV400" },
            new() { Merk = "Kia", Soort = "auto", Kenteken = "QR-123-ST", Kleur = "Zilver", AanschafJaar = 2019, Type = "Sportage" },
            new() { Merk = "Hyundai", Soort = "auto", Kenteken = "UV-456-WX", Kleur = "Blauw", AanschafJaar = 2020, Type = "Tucson" },
            new() { Merk = "Skoda", Soort = "auto", Kenteken = "YZ-789-AB", Kleur = "Groen", AanschafJaar = 2017, Type = "Octavia" },
            new() { Merk = "Mazda", Soort = "auto", Kenteken = "CD-012-EF", Kleur = "Wit", AanschafJaar = 2018, Type = "3" },
            new() { Merk = "Subaru", Soort = "auto", Kenteken = "GH-345-IJ", Kleur = "Grijs", AanschafJaar = 2021, Type = "Impreza" },
            new() { Merk = "Suzuki", Soort = "auto", Kenteken = "KL-678-MN", Kleur = "Zwart", AanschafJaar = 2019, Type = "Vitara" },
            new() { Merk = "Volvo", Soort = "auto", Kenteken = "OP-901-QR", Kleur = "Zilver", AanschafJaar = 2020, Type = "XC60" },
            new() { Merk = "Mitsubishi", Soort = "auto", Kenteken = "ST-234-UV", Kleur = "Rood", AanschafJaar = 2017, Type = "Outlander" },
            new() { Merk = "Jeep", Soort = "auto", Kenteken = "WX-567-YZ", Kleur = "Groen", AanschafJaar = 2022, Type = "Wrangler" },
            new() { Merk = "Land Rover", Soort = "auto", Kenteken = "YZ-890-AB", Kleur = "Blauw", AanschafJaar = 2021, Type = "Defender" },
            new() { Merk = "Bailey", Soort = "auto", Kenteken = "CD-123-EF", Kleur = "Grijs", AanschafJaar = 2018, Type = "Unicorn" },
            new() { Merk = "Lunar", Soort = "auto", Kenteken = "GH-456-IJ", Kleur = "Wit", AanschafJaar = 2020, Type = "Venus" },
            new() { Merk = "Coachman", Soort = "auto", Kenteken = "KL-789-MN", Kleur = "Zilver", AanschafJaar = 2019, Type = "Pastiche" },
            new() { Merk = "Swift", Soort = "auto", Kenteken = "OP-012-QR", Kleur = "Blauw", AanschafJaar = 2017, Type = "Conqueror" },
            new() { Merk = "Elddis", Soort = "auto", Kenteken = "ST-345-UV", Kleur = "Groen", AanschafJaar = 2022, Type = "Avante" },
            new() { Merk = "Compass", Soort = "auto", Kenteken = "ST-345-UV", Kleur = "Blauw", AanschafJaar = 2021, Type = "Casita" },
            new() { Merk = "Coachman", Soort = "auto", Kenteken = "WX-678-YZ", Kleur = "Groen", AanschafJaar = 2016, Type = "VIP" },
            new() { Merk = "Buccaneer", Soort = "auto", Kenteken = "AB-901-CD", Kleur = "Rood", AanschafJaar = 2022, Type = "Commodore" },
            new() { Merk = "Caravelair", Soort = "auto", Kenteken = "EF-234-GH", Kleur = "Zwart", AanschafJaar = 2015, Type = "Allegra" },
            new() { Merk = "Sterckeman", Soort = "auto", Kenteken = "IJ-567-KL", Kleur = "Wit", AanschafJaar = 2021, Type = "StarleƩ" },
            new() { Merk = "Tab", Soort = "auto", Kenteken = "MN-890-OP", Kleur = "Grijs", AanschafJaar = 2018, Type = "320" },
            new() { Merk = "Volkswagen", Soort = "auto", Kenteken = "QR-123-ST", Kleur = "Blauw", AanschafJaar = 2019, Type = "Grand California" },
            new() { Merk = "Mercedes", Soort = "auto", Kenteken = "UV-456-WX", Kleur = "Groen", AanschafJaar = 2020, Type = "Sprinter" },
            new() { Merk = "Ford", Soort = "auto", Kenteken = "YZ-789-AB", Kleur = "Rood", AanschafJaar = 2017, Type = "Transit Custom" },
            new() { Merk = "Fiat", Soort = "auto", Kenteken = "CD-012-EF", Kleur = "Zwart", AanschafJaar = 2018, Type = "Talento" },
            new() { Merk = "Citroen", Soort = "auto", Kenteken = "GH-345-IJ", Kleur = "Wit", AanschafJaar = 2021, Type = "SpaceTourer" },
            new() { Merk = "Peugeot", Soort = "auto", Kenteken = "KL-678-MN", Kleur = "Grijs", AanschafJaar = 2019, Type = "Traveller" },
            new() { Merk = "Renault", Soort = "auto", Kenteken = "OP-901-QR", Kleur = "Blauw", AanschafJaar = 2020, Type = "Trafic" },
            new() { Merk = "Iveco", Soort = "auto", Kenteken = "ST-234-UV", Kleur = "Groen", AanschafJaar = 2017, Type = "Daily" },
            new() { Merk = "Opel", Soort = "auto", Kenteken = "WX-567-YZ", Kleur = "Rood", AanschafJaar = 2022, Type = "Vivaro" },
            new() { Merk = "Nissan", Soort = "auto", Kenteken = "YZ-890-AB", Kleur = "Zwart", AanschafJaar = 2021, Type = "Primastar" },
            new() { Merk = "Toyota", Soort = "auto", Kenteken = "CD-123-EF", Kleur = "Wit", AanschafJaar = 2019, Type = "Yaris" },
            new() { Merk = "Ford", Soort = "auto", Kenteken = "GH-456-IJ", Kleur = "Grijs", AanschafJaar = 2020, Type = "Kuga" },
            new() { Merk = "Volkswagen", Soort = "auto", Kenteken = "KL-789-MN", Kleur = "Blauw", AanschafJaar = 2017, Type = "Passat" },
            new() { Merk = "Honda", Soort = "auto", Kenteken = "OP-012-QR", Kleur = "Groen", AanschafJaar = 2018, Type = "Accord" },
            new() { Merk = "BMW", Soort = "auto", Kenteken = "ST-345-UV", Kleur = "Zilver", AanschafJaar = 2021, Type = "X5" },
            new() { Merk = "Audi", Soort = "auto", Kenteken = "WX-678-YZ", Kleur = "Zwart", AanschafJaar = 2019, Type = "Q7" },
            new() { Merk = "Mercedes", Soort = "auto", Kenteken = "AB-901-CD", Kleur = "Wit", AanschafJaar = 2020, Type = "GLC" },
            new() { Merk = "Nissan", Soort = "auto", Kenteken = "EF-234-GH", Kleur = "Grijs", AanschafJaar = 2017, Type = "Juke" },
            new() { Merk = "Peugeot", Soort = "auto", Kenteken = "IJ-567-KL", Kleur = "Blauw", AanschafJaar = 2022, Type = "308" },
            new() { Merk = "Renault", Soort = "auto", Kenteken = "MN-890-OP", Kleur = "Groen", AanschafJaar = 2021, Type = "Megane" },
            new() { Merk = "Tabbert", Soort = "auto", Kenteken = "QR-123-ST", Kleur = "Rood", AanschafJaar = 2018, Type = "Rossini" },
            new() { Merk = "Dethleffs", Soort = "auto", Kenteken = "UV-456-WX", Kleur = "Zwart", AanschafJaar = 2019, Type = "Beduin" },
            new() { Merk = "Fendt", Soort = "auto", Kenteken = "YZ-789-AB", Kleur = "Wit", AanschafJaar = 2020, Type = "Tendenza" },
            new() { Merk = "Knaus", Soort = "auto", Kenteken = "CD-012-EF", Kleur = "Zilver", AanschafJaar = 2017, Type = "Sudwind" },
            new() { Merk = "Hobby", Soort = "auto", Kenteken = "GH-345-IJ", Kleur = "Blauw", AanschafJaar = 2021, Type = "Excellent" },
            new() { Merk = "Adria", Soort = "auto", Kenteken = "KL-678-MN", Kleur = "Groen", AanschafJaar = 2016, Type = "AcƟon" },
            new() { Merk = "Eriba", Soort = "auto", Kenteken = "OP-901-QR", Kleur = "Rood", AanschafJaar = 2022, Type = "Feeling" },
            new() { Merk = "Burstner", Soort = "auto", Kenteken = "ST-234-UV", Kleur = "Zwart", AanschafJaar = 2015, Type = "Averso" },
            new() { Merk = "LMC", Soort = "auto", Kenteken = "WX-567-YZ", Kleur = "Wit", AanschafJaar = 2021, Type = "Vivo" },
            new() { Merk = "Sprite", Soort = "auto", Kenteken = "YZ-890-AB", Kleur = "Grijs", AanschafJaar = 2018, Type = "Major" },
            new() { Merk = "Volkswagen", Soort = "auto", Kenteken = "CD-123-EF", Kleur = "Zwart", AanschafJaar = 2019, Type = "MulƟvan" },
            new() { Merk = "Mercedes", Soort = "auto", Kenteken = "GH-456-IJ", Kleur = "Wit", AanschafJaar = 2020, Type = "Vito" },
            new() { Merk = "Ford", Soort = "auto", Kenteken = "KL-789-MN", Kleur = "Grijs", AanschafJaar = 2017, Type = "Custom" },
            new() { Merk = "Fiat", Soort = "auto", Kenteken = "OP-012-QR", Kleur = "Blauw", AanschafJaar = 2018, Type = "Scudo" },
            new() { Merk = "Citroen", Soort = "auto", Kenteken = "ST-345-UV", Kleur = "Groen", AanschafJaar = 2021, Type = "Berlingo" },
            new() { Merk = "Peugeot", Soort = "auto", Kenteken = "WX-678-YZ", Kleur = "Rood", AanschafJaar = 2020, Type = "Partner" },
            new() { Merk = "Volkswagen", Soort = "camper", Kenteken = "AB-123-CD", Kleur = "Rood", AanschafJaar = 2018, Type = "California" },
            new() { Merk = "Mercedes", Soort = "camper", Kenteken = "EF-456-GH", Kleur = "Zilver", AanschafJaar = 2019, Type = "Marco Polo" },
            new() { Merk = "Ford", Soort = "camper", Kenteken = "IJ-789-KL", Kleur = "Blauw", AanschafJaar = 2020, Type = "Transit Custom" },
            new() { Merk = "Fiat", Soort = "camper", Kenteken = "MN-012-OP", Kleur = "Wit", AanschafJaar = 2017, Type = "Ducato" },
            new() { Merk = "Citroën", Soort = "camper", Kenteken = "QR-345-ST", Kleur = "Grijs", AanschafJaar = 2021, Type = "Jumper" },
            new() { Merk = "Peugeot", Soort = "camper", Kenteken = "UV-678-WX", Kleur = "Zwart", AanschafJaar = 2016, Type = "Boxer" },
            new() { Merk = "Renault", Soort = "camper", Kenteken = "YZ-901-AB", Kleur = "Groen", AanschafJaar = 2022, Type = "Master" },
            new() { Merk = "Nissan", Soort = "camper", Kenteken = "CD-234-EF", Kleur = "Blauw", AanschafJaar = 2015, Type = "NV400" },
            new() { Merk = "Opel", Soort = "camper", Kenteken = "GH-567-IJ", Kleur = "Zilver", AanschafJaar = 2021, Type = "Movano" },
            new() { Merk = "Iveco", Soort = "camper", Kenteken = "KL-890-MN", Kleur = "Rood", AanschafJaar = 2018, Type = "Daily" },
            new() { Merk = "Volkswagen", Soort = "camper", Kenteken = "OP-123-QR", Kleur = "Wit", AanschafJaar = 2017, Type = "Grand California" },
            new() { Merk = "Mercedes", Soort = "camper", Kenteken = "ST-456-UV", Kleur = "Blauw", AanschafJaar = 2019, Type = "Sprinter" },
            new() { Merk = "Ford", Soort = "camper", Kenteken = "WX-789-YZ", Kleur = "Zwart", AanschafJaar = 2020, Type = "Nugget" },
            new() { Merk = "Fiat", Soort = "camper", Kenteken = "AB-012-CD", Kleur = "Groen", AanschafJaar = 2016, Type = "Talento" },
            new() { Merk = "Citroën", Soort = "camper", Kenteken = "EF-345-GH", Kleur = "Rood", AanschafJaar = 2018, Type = "SpaceTourer" },
            new() { Merk = "Peugeot", Soort = "camper", Kenteken = "IJ-678-KL", Kleur = "Zwart", AanschafJaar = 2021, Type = "Traveller" },
            new() { Merk = "Renault", Soort = "camper", Kenteken = "MN-901-OP", Kleur = "Wit", AanschafJaar = 2020, Type = "Trafic" },
            new() { Merk = "Nissan", Soort = "camper", Kenteken = "QR-234-ST", Kleur = "Zilver", AanschafJaar = 2019, Type = "Primastar" },
            new() { Merk = "Opel", Soort = "camper", Kenteken = "UV-567-WX", Kleur = "Grijs", AanschafJaar = 2022, Type = "Vivaro" },
            new() { Merk = "Iveco", Soort = "camper", Kenteken = "YZ-890-AB", Kleur = "Zwart", AanschafJaar = 2017, Type = "Eurocargo" },
            new() { Merk = "Volkswagen", Soort = "camper", Kenteken = "CD-123-EF", Kleur = "Blauw", AanschafJaar = 2018, Type = "Multivan" },
            new() { Merk = "Mercedes", Soort = "camper", Kenteken = "GH-456-IJ", Kleur = "Groen", AanschafJaar = 2020, Type = "Vito" },
            new() { Merk = "Ford", Soort = "camper", Kenteken = "KL-789-MN", Kleur = "Zilver", AanschafJaar = 2017, Type = "Kuga Camper" },
            new() { Merk = "Fiat", Soort = "camper", Kenteken = "OP-012-QR", Kleur = "Rood", AanschafJaar = 2018, Type = "Scudo" },
            new() { Merk = "Citroën", Soort = "camper", Kenteken = "ST-345-UV", Kleur = "Wit", AanschafJaar = 2019, Type = "Berlingo" },
            new() { Merk = "Peugeot", Soort = "camper", Kenteken = "WX-678-YZ", Kleur = "Grijs", AanschafJaar = 2016, Type = "Expert Camper" },
            new() { Merk = "Renault", Soort = "camper", Kenteken = "AB-901-CD", Kleur = "Blauw", AanschafJaar = 2022, Type = "Kangoo Camper" },
            new() { Merk = "Nissan", Soort = "camper", Kenteken = "EF-234-GH", Kleur = "Zwart", AanschafJaar = 2015, Type = "Juke Camper" },
            new() { Merk = "Opel", Soort = "camper", Kenteken = "GH-567-IJ", Kleur = "Groen", AanschafJaar = 2021, Type = "Zafira Camper" },
            new() { Merk = "Iveco", Soort = "camper", Kenteken = "KL-890-MN", Kleur = "Rood", AanschafJaar = 2018, Type = "Camper 2000" },
            new() { Merk = "Volkswagen", Soort = "camper", Kenteken = "OP-123-QR", Kleur = "Zwart", AanschafJaar = 2017, Type = "Kombi" },
            new() { Merk = "Mercedes", Soort = "camper", Kenteken = "ST-456-UV", Kleur = "Zilver", AanschafJaar = 2021, Type = "Sprinter XXL" },
            new() { Merk = "Ford", Soort = "camper", Kenteken = "WX-789-YZ", Kleur = "Blauw", AanschafJaar = 2020, Type = "Custom Camper" },
            new() { Merk = "Fiat", Soort = "camper", Kenteken = "AB-012-CD", Kleur = "Wit", AanschafJaar = 2016, Type = "Ducato Maxi" },
            new() { Merk = "Citroën", Soort = "camper", Kenteken = "EF-345-GH", Kleur = "Groen", AanschafJaar = 2018, Type = "Jumper Camper" },
            new() { Merk = "Peugeot", Soort = "camper", Kenteken = "IJ-678-KL", Kleur = "Zwart", AanschafJaar = 2021, Type = "Boxer XL" },
            new() { Merk = "Renault", Soort = "camper", Kenteken = "MN-901-OP", Kleur = "Grijs", AanschafJaar = 2019, Type = "Master Pro" },
            new() { Merk = "Nissan", Soort = "camper", Kenteken = "QR-234-ST", Kleur = "Blauw", AanschafJaar = 2022, Type = "NV300 Camper" },
            new() { Merk = "Opel", Soort = "camper", Kenteken = "UV-567-WX", Kleur = "Zilver", AanschafJaar = 2017, Type = "Vivaro XL" },
            new() { Merk = "Iveco", Soort = "camper", Kenteken = "YZ-890-AB", Kleur = "Rood", AanschafJaar = 2018, Type = "Daily Pro" },
            new() { Merk = "Volkswagen", Soort = "camper", Kenteken = "CD-123-EF", Kleur = "Blauw", AanschafJaar = 2018, Type = "Transporter" },
            new() { Merk = "Mercedes", Soort = "camper", Kenteken = "GH-456-IJ", Kleur = "Zwart", AanschafJaar = 2019, Type = "V-Class Camper" },
            new() { Merk = "Ford", Soort = "camper", Kenteken = "KL-789-MN", Kleur = "Wit", AanschafJaar = 2022, Type = "Transit Nugget Plus" },
            new() { Merk = "Hobby", Soort = "caravan", Kenteken = "AB-123-CD", Kleur = "Wit", AanschafJaar = 2018, Type = "De Luxe" },
            new() { Merk = "Fendt", Soort = "caravan", Kenteken = "EF-456-GH", Kleur = "Grijs", AanschafJaar = 2019, Type = "Bianco" },
            new() { Merk = "Knaus", Soort = "caravan", Kenteken = "IJ-789-KL", Kleur = "Blauw", AanschafJaar = 2020, Type = "Sport" },
            new() { Merk = "Adria", Soort = "caravan", Kenteken = "MN-012-OP", Kleur = "Zilver", AanschafJaar = 2017, Type = "Altea" },
            new() { Merk = "Dethleffs", Soort = "caravan", Kenteken = "QR-345-ST", Kleur = "Groen", AanschafJaar = 2021, Type = "Camper" },
            new() { Merk = "Tabbert", Soort = "caravan", Kenteken = "UV-678-WX", Kleur = "Zwart", AanschafJaar = 2016, Type = "Puccini" },
            new() { Merk = "Burstner", Soort = "caravan", Kenteken = "YZ-901-AB", Kleur = "Wit", AanschafJaar = 2022, Type = "Premio" },
            new() { Merk = "LMC", Soort = "caravan", Kenteken = "CD-234-EF", Kleur = "Blauw", AanschafJaar = 2015, Type = "Musica" },
            new() { Merk = "Sprite", Soort = "caravan", Kenteken = "GH-567-IJ", Kleur = "Rood", AanschafJaar = 2021, Type = "Cruzer" },
            new() { Merk = "Bailey", Soort = "caravan", Kenteken = "KL-890-MN", Kleur = "Grijs", AanschafJaar = 2018, Type = "Unicorn" },
            new() { Merk = "Lunar", Soort = "caravan", Kenteken = "OP-123-QR", Kleur = "Zilver", AanschafJaar = 2017, Type = "Clubman" },
            new() { Merk = "SwiŌ", Soort = "caravan", Kenteken = "ST-456-UV", Kleur = "Blauw", AanschafJaar = 2019, Type = "Conqueror" },
            new() { Merk = "Compass", Soort = "caravan", Kenteken = "WX-789-YZ", Kleur = "Groen", AanschafJaar = 2020, Type = "Casita" },
            new() { Merk = "Coachman", Soort = "caravan", Kenteken = "AB-012-CD", Kleur = "Rood", AanschafJaar = 2016, Type = "VIP" },
            new() { Merk = "Buccaneer", Soort = "caravan", Kenteken = "EF-345-GH", Kleur = "Zwart", AanschafJaar = 2018, Type = "Commodore" },
            new() { Merk = "Caravelair", Soort = "caravan", Kenteken = "IJ-678-KL", Kleur = "Wit", AanschafJaar = 2021, Type = "Allegra" },
            new() { Merk = "Sterckeman", Soort = "caravan", Kenteken = "MN-901-OP", Kleur = "Zilver", AanschafJaar = 2020, Type = "StarleƩ" },
            new() { Merk = "Tab", Soort = "caravan", Kenteken = "QR-234-ST", Kleur = "Blauw", AanschafJaar = 2019, Type = "320" },
            new() { Merk = "Eriba", Soort = "caravan", Kenteken = "UV-567-WX", Kleur = "Grijs", AanschafJaar = 2022, Type = "Touring" },
            new() { Merk = "Adria", Soort = "caravan", Kenteken = "YZ-890-AB", Kleur = "Rood", AanschafJaar = 2017, Type = "AcƟon" },
            new() { Merk = "Fendt", Soort = "caravan", Kenteken = "CD-123-EF", Kleur = "Wit", AanschafJaar = 2018, Type = "Tendenza" },
            new() { Merk = "Knaus", Soort = "caravan", Kenteken = "GH-456-IJ", Kleur = "Groen", AanschafJaar = 2020, Type = "Sudwind" },
            new() { Merk = "Hobby", Soort = "caravan", Kenteken = "KL-789-MN", Kleur = "Zwart", AanschafJaar = 2017, Type = "Excellent" },
            new() { Merk = "Dethleffs", Soort = "caravan", Kenteken = "OP-012-QR", Kleur = "Blauw", AanschafJaar = 2019, Type = "Beduin" },
            new() { Merk = "Burstner", Soort = "caravan", Kenteken = "ST-345-UV", Kleur = "Zilver", AanschafJaar = 2021, Type = "Averso" },
            new() { Merk = "LMC", Soort = "caravan", Kenteken = "WX-678-YZ", Kleur = "Rood", AanschafJaar = 2020, Type = "Vivo" },
            new() { Merk = "Sprite", Soort = "caravan", Kenteken = "AB-901-CD", Kleur = "Groen", AanschafJaar = 2019, Type = "Major" },
            new() { Merk = "Bailey", Soort = "caravan", Kenteken = "EF-234-GH", Kleur = "Wit", AanschafJaar = 2022, Type = "Phoenix" },
            new() { Merk = "Lunar", Soort = "caravan", Kenteken = "GH-567-IJ", Kleur = "Grijs", AanschafJaar = 2017, Type = "Delta" },
            new() { Merk = "SwiŌ", Soort = "caravan", Kenteken = "KL-890-MN", Kleur = "Zwart", AanschafJaar = 2018, Type = "Elegance" },
            new() { Merk = "Compass", Soort = "caravan", Kenteken = "OP-123-QR", Kleur = "Blauw", AanschafJaar = 2021, Type = "Corona" },
            new() { Merk = "Coachman", Soort = "caravan", Kenteken = "ST-456-UV", Kleur = "Zilver", AanschafJaar = 2019, Type = "Acadia" },
            new() { Merk = "Buccaneer", Soort = "caravan", Kenteken = "WX-789-YZ", Kleur = "Rood", AanschafJaar = 2020, Type = "Barracuda" },
            new() { Merk = "Caravelair", Soort = "caravan", Kenteken = "AB-012-CD", Kleur = "Groen", AanschafJaar = 2016, Type = "Antares" },
            new() { Merk = "Sterckeman", Soort = "caravan", Kenteken = "EF-345-GH", Kleur = "Zwart", AanschafJaar = 2018, Type = "Evolution" },
            new() { Merk = "Tab", Soort = "caravan", Kenteken = "IJ-678-KL", Kleur = "Zilver", AanschafJaar = 2021, Type = "400" },
            new() { Merk = "Eriba", Soort = "caravan", Kenteken = "MN-901-OP", Kleur = "Blauw", AanschafJaar = 2020, Type = "Nova" },
            new() { Merk = "Adria", Soort = "caravan", Kenteken = "QR-234-ST", Kleur = "Wit", AanschafJaar = 2019, Type = "Adora" },
            new() { Merk = "Fendt", Soort = "caravan", Kenteken = "UV-567-WX", Kleur = "Zwart", AanschafJaar = 2022, Type = "Opal" },
            new() { Merk = "Knaus", Soort = "caravan", Kenteken = "YZ-890-AB", Kleur = "Groen", AanschafJaar = 2017, Type = "Sky Traveller" },
            new() { Merk = "Hobby", Soort = "caravan", Kenteken = "CD-123-EF", Kleur = "Grijs", AanschafJaar = 2018,Type = "Prestige" },
            new() { Merk = "Dethleffs", Soort = "caravan", Kenteken = "GH-456-IJ", Kleur = "Blauw", AanschafJaar = 2020, Type = "C`go" },
            new() { Merk = "Burstner", Soort = "caravan", Kenteken = "KL-789-MN", Kleur = "Rood", AanschafJaar = 2017, Type = "Premio Life" }
            };

            var voertuigen = voertuigDtos.Select(dto => new Voertuig
            {
                Merk = dto.Merk,
                Kenteken = dto.Kenteken,
                Kleur = dto.Kleur,
                Type = dto.Type,
                AanschafJaar = dto.AanschafJaar,
                Soort = dto.Soort,
            }).ToList();

            Random random = new();
            decimal basisKilometerPrijs = 0.25m;
            foreach (Voertuig voertuig in voertuigen)
            {
                decimal kilometerPrijsVariatie = (decimal)(random.NextDouble() * 0.10 - 0.05); 
                decimal kilometerPrijs = basisKilometerPrijs + kilometerPrijsVariatie;
                voertuig.VoertuigData = new VoertuigData   
                {   
                    VoertuigId = voertuig.VoertuigId,
                    Status = VoertuigStatussen.KlaarVoorGebruik,
                    KilometerPrijs = Math.Round(kilometerPrijs, 2)
                };
            }
            await voertuigService.AddVoertuigen(voertuigen);
        }
        
    }
}
