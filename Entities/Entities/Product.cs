using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    [Table("PRODUCT")]
    public class Product
    {

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        public string DescriptionProduct { get; set; }

        public int Stock { get; set; }

        public int UnitPrice { get; set; }

        /*public List<Demand> Demands { get; private set; } = new List<Demand>(); // Inicialização para evitar null reference*/


        /// <summary>
        ///abaixo configurar fk
        /// </summary>
        public virtual Company Company { get; set; }
        public string CompanyId { get; set; }

    }
}
