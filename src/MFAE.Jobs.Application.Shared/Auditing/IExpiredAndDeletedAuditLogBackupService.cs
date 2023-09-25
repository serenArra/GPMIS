using System.Collections.Generic;
using Abp.Auditing;

namespace MFAE.Jobs.Auditing
{
    public interface IExpiredAndDeletedAuditLogBackupService
    {
        bool CanBackup();
        
        void Backup(List<AuditLog> auditLogs);
    }
}