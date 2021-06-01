using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public partial class Registrados
{
    [JsonProperty("registrados")]
    public Dictionary<string, Registrado> RegistradosRegistrados { get; set; }
}

public partial class Registrado
{
    public Registrado(string idPersona, int age, string firstname, int idZona, string identityCard, string lastname, string timeIn, string timeOut)
    {
        this.idPersona = idPersona;
        Age = age;
        Firstname = firstname;
        IdZona = idZona;
        IdentityCard = identityCard;
        Lastname = lastname;
        TimeIn = timeIn;
        TimeOut = timeOut;
    }

    public string idPersona { get; set; }
    [JsonProperty("age")]
   // [JsonConverter(typeof(ParseStringConverter))]
    public int Age { get; set; }

    [JsonProperty("firstname")]
    public string Firstname { get; set; }

    [JsonProperty("idZona")]
    // [JsonConverter(typeof(ParseStringConverter))]
    public int IdZona { get; set; }

    [JsonProperty("identityCard")]
    public string IdentityCard { get; set; }

    [JsonProperty("lastname")]
    public string Lastname { get; set; }

    [JsonProperty("timeIn")]
    public string TimeIn { get; set; }

    [JsonProperty("timeOut")]
    public string TimeOut { get; set; }

    public string NombreZonaPerson { get; set; }

    public string NombreEventoPerson { get; set; }

    public string EventoActivoPerson { get; set; }
}
