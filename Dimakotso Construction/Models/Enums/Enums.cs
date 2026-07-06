using System.ComponentModel;

namespace Dimakotso_Construction.Models.Enums
{
    public enum EquityCode
    {
        [Description("Black African")] BA,
        [Description("Coloured")] BC,
        [Description("Indian/Asian")] BI,
        [Description("White")] WH,
        [Description("Other")] Oth
    }

    public enum CitizenStatusCode
    {
        [Description("SA Citizen")] SACitizen = 03,
        [Description("Permanent Resident")] PermanentResident = 01,
        [Description("Refugee")] Refugee = 02,
        [Description("Dual Citizen")] Dual = 04,
        [Description("Unknown")] Unknown = 00
    }

    public enum DisabilityCode
    {
        [Description("None")] None,
        [Description("Sight Difficulty")] Sight = 01,
        [Description("Hearing Difficulty")] Hearing = 02,
        [Description("Communication")] Communication = 03,
        [Description("Physical")] Physical = 04,
        [Description("Intellectual")] Intellectual = 05,
        [Description("Emotional")] Emotional = 06,
        [Description("Multiple Difficulties")] Multiple = 07
    }

    public enum EmploymentStatus
    {
        [Description("Employed")] Employed = 01,
        [Description("Unemployed, looking for work")] Unemployed = 02,
        [Description("Not working – Student/Scholar")] Student = 06,
        [Description("Not working – Disabled Person")] Disabled = 08
    }

    public enum EnrollmentStatus
    {
        Registered,
        ActiveTraining,
        FisaPreparation,
        FisaCompleted,
        EisaEligible,
        Certified,
        Withdrawn
    }
}