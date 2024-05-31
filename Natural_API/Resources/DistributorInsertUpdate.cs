namespace Natural_API.Resources
{
    public class DistributorInsertUpdate
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PresignedUrl { get; set; }
        public IFormFile ProfileImage { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
