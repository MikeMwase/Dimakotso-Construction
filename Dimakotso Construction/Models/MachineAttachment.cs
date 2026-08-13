namespace Dimakotso_Construction.Models
{
    public class MachineAttachment
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., "A", "C", "F"
        public string Name { get; set; } = string.Empty; // e.g., "Side-Shift", "Crane Hook"
    }

    public class CertificateAttachment
    {
        public int CertificateId { get; set; }
        public virtual Certificate Certificate { get; set; } = null!;

        public int MachineAttachmentId { get; set; }
        public virtual MachineAttachment MachineAttachment { get; set; } = null!;
    }

    public class MachineRestriction
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., "R1", "R2"
        public string Description { get; set; } = string.Empty; // e.g., "Maximum Load Capacity"
    }

    public class CertificateRestriction
    {
        public int CertificateId { get; set; }
        public virtual Certificate Certificate { get; set; } = null!;

        public int MachineRestrictionId { get; set; }
        public virtual MachineRestriction MachineRestriction { get; set; } = null!;
    }
}