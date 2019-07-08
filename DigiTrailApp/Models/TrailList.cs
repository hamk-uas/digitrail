namespace DigiTrailApp.Models
{
    /// <summary>
    /// TrailList model
    /// </summary>
    public class TrailList
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public TrailList(string Id, /*string Difficulty,*/ string Name, string Description)
        {
            this.Id = Id;
            //this.Difficulty = Difficulty;
            this.Name = Name;
            this.Description = Description;
        }
    }
}