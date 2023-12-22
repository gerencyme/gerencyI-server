namespace ApiAuthentication.Views
{
    [Serializable]
    public class ZipCodeView
    {
        public string Code { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string? StreetType { get; set; }
        public string? Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string? Complement { get; set; }

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
        }
    }
}
