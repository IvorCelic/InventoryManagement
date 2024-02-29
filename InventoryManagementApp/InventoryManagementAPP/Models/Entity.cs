using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPP.Models
{
    /// <summary>
    /// Ovo je vršna nad klasa koja služi za osnovne atribute tipa šifra, operater, datum unosa, promjene, itd.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// This attribute is primary key in database with generating value identities (1, 1)
        /// </summary>
        [Key]
        public int? Id { get; set; }
    }
}
