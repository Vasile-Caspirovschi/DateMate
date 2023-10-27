using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Group
    {
        public Group()
        {
        }

        public Group(string name)
        {
            Name = name;
        }

        [Key]
        public string Name { get; set; } = string.Empty;
        public List<Connection> Connections { get; set; } = new();
    }
}