using System.ComponentModel.DataAnnotations;

namespace RasDialNet
{
    class RasDialOptions
    {
        [Required]
        public string Action { get; set; }

        [Required]
        public string ConnectionName { get; set; }
    }
}