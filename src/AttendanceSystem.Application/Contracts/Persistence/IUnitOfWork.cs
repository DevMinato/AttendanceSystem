using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        Task<int> SaveChangesAsync();
        void Commit();
        void Rollback();

        IRepository<ApplicationUser> ApplicationUserRepository { get; }
        IRepository<Member> MemberRepository { get; }
        IRepository<FollowUpReport> FollowupReportRepository { get; }
        IRepository<FollowUpDetail> FollowupDetailRepository { get; }
        IRepository<OutreachReport> OutreachReportRepository { get; }
        IRepository<OutreachDetail> OutreachDetailRepository { get; }
    }
}