using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MT_NetCore_Data.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public string Team_Ref { get; set; }
        
        public string OfflineId { get; set; }

        //public TeamStatus Status { get; set; }

        //[DefaultValue(PaymentStatus.NothingToPay)]
        //public PaymentStatus paystatus { get; set; }

        public Int64 MaxUsers { get; set; }
        public Int64 MaxRecord { get; set; }
        public Int64 MaxRecordsPerMth { get; set; }
        public Int64 RecordsThisMth { get; set; }
        public Int64 RecordsThisYear { get; set; }
        public Int64 MaxForms { get; set; }
        public string Industry { get; set; }
        public string TeamSize { get; set; }
      
        public DateTime DatePaid { get; set; }

        public DateTime ResetDate { get; set; }

        public DateTime NextSubscriptionDate { get; set; }

        public DateTime LastApiTimestamp { get; set; }

        public DateTime PaymentLastApiTimestamp { get; set; }
      

        public string SubscriptionID { get; set; }

        public string CustomerSubscriptionID { get; set; }

        // public string[] transactionRef { get; set; }

        public bool DisplayCampaignTab { get; set; }

        public bool DisplayReportTab { get; set; }

        public string Country { get; set; }
        public string Currency { get; set; }
        [DefaultValue("/assets/images/logo.png")]

        public string LogoLink { get; set; }

        public string CustomerAquisition { get; set; }

    }
}
