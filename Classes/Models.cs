using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace sportnoDrustvo.Classes
{
    public class Models
    {
        public class Termin
        {
            public int Id { get; set; }
            public string ImeEkipe { get; set; }
            public DateTime DatumTermina { get; set; }
            public int MaxUdelezencev { get; set; }
            //done
        }

        //definicija modela obbv.
        public class Obvestilo
        {
            public int Id { get; set; }
            public int TerminId { get; set; }
            public bool Obvescanje { get; set; }
            public Termin Termin { get; set; }
            //done
        }

        //definicija modela rekvizit
        public class Rekvizit
        {
            public int Id { get; set; }
            public string Ime { get; set; }
            public bool JeNaVoljo { get; set; }
            public int? ClanId { get; set; } // Opcijsko, če je rekvizit izposojen
            public DateTime? DatumIzposoje { get; set; } // Opcijsko, če je rekvizit izposojen
            public Clan Clan { get; set; } // Povezava do člana, ki je izposodil rekvizit

        }

        //definicija modela trener
        public class Trener
        {
            public int Id { get; set; }
            public string Ime { get; set; }
            public string Specializacija { get; set; }
            //done
        }

        
        public class Clan
        {
            public int Id { get; set; }
            public string Ime { get; set; }
            public string Priimek { get; set; }
            public ICollection<Obvestilo> Obvestila { get; set; }
            //done
        }

        public class Rezervacija
        {
            public int Id { get; set; }
            public int TerminId { get; set; }
            public int ClanId { get; set; }
            public DateTime DatumRezervacije { get; set; }
            public Termin Termin { get; set; }
            public Clan Clan { get; set; }
            //done
        }
    }
}
