using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Wba.StovePalace.Models
{
    public class Stove
    {
        public int Id { get; set; }

        [ForeignKey("Brand")]
        [Display(Name = "Merk")]
        public int BrandId { get; set; }

        [Display(Name = "Merk")]
        public Brand Brand { get; set; }

        [ForeignKey("Fuel")]
        [Display(Name = "Brandstof")]
        public int FuelId { get; set; }
        [Display(Name = "Brandstof")]
        public Fuel Fuel { get; set; }

        [Display(Name = "Modelnaam")]
        [Required(ErrorMessage = "Geef een model(naam) op !")]
        [StringLength(30, ErrorMessage = "Modelnaam maximaal 30 letters")]
        public string ModelName { get; set; }

        [Display(Name = "Vermogen")]
        [Required(ErrorMessage = "Voer een waarde in tussen 500 en 20000 !")]
        [Range(500, 20000, ErrorMessage = "Kies een waarde tussen 200 en 20.000")]
        public int Performance { get; set; }

        [Display(Name = "Prijs")]
        [Required(ErrorMessage = "Voer een waarde in tussen 1 en 20000")]
        [Range(1, 20000, ErrorMessage = "Kies een waarde tussen 1 en 20000")]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal SalesPrice { get; set; }

        public string ImagePath { get; set; }

    }
}
