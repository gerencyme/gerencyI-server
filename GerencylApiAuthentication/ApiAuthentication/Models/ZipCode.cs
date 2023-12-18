namespace ApiAuthentication.Models
{
    [Serializable]
    public class ZipCode
    {
        private string _code;

        public string Code
        {
            get => _code;
            set
            {
                if (ValidateZipCode(value))
                {
                    _code = value;
                }
                else
                {
                    throw new ("Invalid ZIP code.");
                }
            }
        }

        public string Street { get; set; }
        public string Number { get; set; }
        public string StreetType { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Complement { get; set; }

        public string FullAddress
        {
            get
            {
                string address = $"{StreetType} {Street}, {Number}";
                if (!string.IsNullOrEmpty(Complement))
                {
                    address += $" - {Complement}";
                }
                address += $", {Neighborhood}, {City} - {State}, {Country}";
                return address;
            }
            set {}
        }

        public ZipCode(string code, string street, string number, string streetType, string neighborhood, string city, string state, string country, string complement = null)
        {
            Code = code;
            Street = street;
            Number = number;
            StreetType = streetType;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            Country = country;
            Complement = complement;
        }

        public ZipCode()
        {

        }

        public string FormatZipCode()
        {
            return $"{Code.Substring(0, 5)}-{Code.Substring(5)}";
        }

        public bool ValidateZipCode(string zipCode)
        {
            // For example, check if the ZIP code has the expected format and is in a valid range for the country/state
            return !string.IsNullOrEmpty(zipCode) && zipCode.Length == 8 && int.TryParse(zipCode, out _);
        }
    }
}