using System.ComponentModel.DataAnnotations;

namespace Rakna.Common.Enum
{
    public enum ComplaintType
    {
        [Display(Name = "Other")]
        Other,

        [Display(Name = "System Bug")]
        SystemError,

        [Display(Name = "Billing")]
        BillingError,

        [Display(Name = "Service Delay")]
        ServiceDelay,

        [Display(Name = "Equipment Issue")]
        EquipmentIssue,

        [Display(Name = "Policy Violation")]
        PolicyViolation,

        [Display(Name = "Customer Feedback")]
        CustomerFeedback
    }
}