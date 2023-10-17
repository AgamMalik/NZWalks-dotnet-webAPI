namespace NZWalks.API.Models.DTO
{

    // this region dto will have the properties that we want to expose back to the client
    public class RegionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
