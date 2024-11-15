namespace RentSmart.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using RentSmart.Data.Common.Models;

    public class Rating : BaseDeletableModel<int>
    {
        public int ConditionAndMaintenanceRate { get; set; }

        public int Location { get; set; }

        public int ValueForMoney { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double AverageRating { get; private set; }

        public virtual Rental Rental { get; set; } = null!;
    }
}
