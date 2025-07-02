using Microsoft.AspNetCore.Identity;

namespace CareerBuilder.API.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties for future job-related features
    public virtual ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
}

// Job posting model
public class JobPosting
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty; // Full-time, Part-time, Contract, Remote
    public string Category { get; set; } = string.Empty; // technology, marketing, sales, etc.
    public string Requirements { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Foreign key
    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser User { get; set; } = null!;
}

public class AdminCostCenter
{
    public string costCenterId { get; set; }
    public string ouCode { get; set; }
    public string costCenterNumber { get; set; }
    public string description { get; set; }
    public string displayName { get; set; }
}

public class Attachments
{
    public int maxItems { get; set; }
    public List<Value> value { get; set; }
}

public class BeelineEnterpriseRates
{
    public string rateCardCalculation { get; set; }
    public int markup { get; set; }
    public bool markupIsPercent { get; set; }
    public List<BillRate> billRates { get; set; }
    public List<PayRate> payRates { get; set; }
}

public class BeelineProfessionalRates
{
    public int billRateMinimum { get; set; }
    public int billRateMaximum { get; set; }
    public int payRateMinimum { get; set; }
    public int payRateMaximum { get; set; }
    public string rateType { get; set; }
    public string ratePeriod { get; set; }
}

public class BillRate
{
    public string name { get; set; }
    public int rate { get; set; }
    public string rateId { get; set; }
    public string rateType { get; set; }
    public int minRate { get; set; }
    public int weekendRate { get; set; }
    public string earningCodeType { get; set; }
    public string procurementRateOverrideType { get; set; }
    public int procurementRateOverrideValue { get; set; }
    public string shift { get; set; }
    public object description { get; set; }
}

public class BillToCostCenter
{
    public string costCenterId { get; set; }
    public string ouCode { get; set; }
    public string costCenterNumber { get; set; }
    public string description { get; set; }
    public string displayName { get; set; }
}

public class BusinessOrganization
{
    public string organizationId { get; set; }
    public string organizationCode { get; set; }
    public string organizationType { get; set; }
    public string displayName { get; set; }
    public string description { get; set; }
}

public class CustomField
{
    public string referenceId { get; set; }
    public string value { get; set; }
    public string name { get; set; }
    public string type { get; set; }
}

public class Experience
{
    public string id { get; set; }
    public string name { get; set; }
    public string code { get; set; }
    public string description { get; set; }
}

public class ExtendedSkills
{
    public List<Skill> skills { get; set; }
    public string additionalSkills { get; set; }
}

public class HiringManager
{
    public string userId { get; set; }
    public string userName { get; set; }
}

public class JobClassification
{
    public string jobTitleId { get; set; }
    public string name { get; set; }
    public string jobTitleCode { get; set; }
    public string jobClassName { get; set; }
}

public class Level
{
    public string id { get; set; }
    public string name { get; set; }
    public string code { get; set; }
    public string description { get; set; }
}

public class MspOwner
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
}

public class Notes
{
    public List<Value> value { get; set; }
}

public class PayRate
{
    public string name { get; set; }
    public int rate { get; set; }
    public string rateId { get; set; }
    public string rateType { get; set; }
    public int minRate { get; set; }
    public int weekendRate { get; set; }
    public string earningCodeType { get; set; }
    public string procurementRateOverrideType { get; set; }
    public int procurementRateOverrideValue { get; set; }
    public string shift { get; set; }
}

public class PhysicalWorkLocation
{
    public string locationId { get; set; }
    public string locationCode { get; set; }
    public string street1 { get; set; }
    public string street2 { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string postalCode { get; set; }
    public string countryCode { get; set; }
}

public class ResourceType
{
    public string code { get; set; }
    public string name { get; set; }
}

public class Root
{
    public string uid { get; set; }
    public string status { get; set; }
    public string sourceVMS { get; set; }
    public int requestedWorkers { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string expiryDate { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    public string location { get; set; }
    public string enterpriseJobPostingNumber { get; set; }
    public string classificationName { get; set; }
    public string managerName { get; set; }
    public string enterpriseName { get; set; }
    public string clientName { get; set; }
    public string clientUid { get; set; }
    public string currency { get; set; }
    public DateTime created { get; set; }
    public DateTime modified { get; set; }
    public string supplierName { get; set; }
    public string supplierUid { get; set; }
    public BeelineProfessionalRates beelineProfessionalRates { get; set; }
    public BeelineEnterpriseRates beelineEnterpriseRates { get; set; }
    public Notes notes { get; set; }
    public JobClassification jobClassification { get; set; }
    public List<CustomField> customFields { get; set; }
    public List<string> supplierEnterpriseConnections { get; set; }
    public HiringManager hiringManager { get; set; }
    public AdminCostCenter adminCostCenter { get; set; }
    public BillToCostCenter billToCostCenter { get; set; }
    public TaxLocation taxLocation { get; set; }
    public PhysicalWorkLocation physicalWorkLocation { get; set; }
    public string industryName { get; set; }
    public string requestType { get; set; }
    public List<ResourceType> resourceTypes { get; set; }
    public List<string> skills { get; set; }
    public WorkAddress workAddress { get; set; }
    public string commentsForSuppliers { get; set; }
    public int hoursPerWeek { get; set; }
    public int quantity { get; set; }
    public MspOwner mspOwner { get; set; }
    public Attachments attachments { get; set; }
    public DateTime releasedToSupplierDate { get; set; }
    public ExtendedSkills extendedSkills { get; set; }
    public int estimatedOvertimeHoursPerWeek { get; set; }
    public int candidateSubmissionLimitPerSupplier { get; set; }
    public BusinessOrganization businessOrganization { get; set; }
    public DateTime submissionDate { get; set; }
}

public class Skill
{
    public string name { get; set; }
    public string code { get; set; }
    public string description { get; set; }
    public bool isRequired { get; set; }
    public Experience experience { get; set; }
    public Level level { get; set; }
}

public class TaxLocation
{
    public string locationId { get; set; }
    public string locationCode { get; set; }
    public string street1 { get; set; }
    public string street2 { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string postalCode { get; set; }
    public string countryCode { get; set; }
}

public class Value
{
    public string text { get; set; }
    public DateTime date { get; set; }
    public string attachmentId { get; set; }
    public string name { get; set; }
    public DateTime createDate { get; set; }
    public DateTime modifyDate { get; set; }
    public string fileName { get; set; }
    public int contentLength { get; set; }
    public string mimeType { get; set; }
    public string extension { get; set; }
}

public class WorkAddress
{
    public string locationId { get; set; }
    public string locationCode { get; set; }
    public string street1 { get; set; }
    public string street2 { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string postalCode { get; set; }
    public string countryCode { get; set; }
}