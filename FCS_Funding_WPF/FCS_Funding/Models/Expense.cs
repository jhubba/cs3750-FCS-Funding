namespace FCS_Funding.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Expense")]
    public partial class Expense
    {
        public int ExpenseID { get; set; }

        public int ExpenseTypeID { get; set; }

        public int? DonationID { get; set; }

        public int? PatientID { get; set; }

        public int? AppointmentID { get; set; }

        [Column(TypeName = "date")]
        public DateTime ExpenseDueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpensePaidDate { get; set; }

        [Column(TypeName = "money")]
        public decimal DonorBill { get; set; }

        [Column(TypeName = "money")]
        public decimal PatientBill { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalExpenseAmount { get; set; }

        public virtual Appointment Appointment { get; set; }

        public virtual Donation Donation { get; set; }

        public virtual ExpenseType ExpenseType { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
