using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace AttendanceSystem.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        //private Repository<UserCategory> _userCategoryRepository;
        private Repository<ApplicationUser> _applicationUserRepository;
        private Repository<Member> _memberRepository;
        private Repository<FollowUpReport> _followupReportRepository;
        private Repository<FollowUpDetail> _followupDetailRepository;
        private Repository<OutreachReport> _outreachReportRepository;
        private Repository<OutreachDetail> _outreachDetailRepository;

        private IDbContextTransaction _transaction;


        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /*public IRepository<UserCategory> UserCategoryRepository =>
                        _userCategoryRepository ?? new Repository<UserCategory>(_dbContext);*/

        
        public IRepository<ApplicationUser> ApplicationUserRepository =>
                            _applicationUserRepository ?? new Repository<ApplicationUser>(_dbContext);

        public IRepository<Member> MemberRepository => _memberRepository ?? new Repository<Member>(_dbContext);
        public IRepository<FollowUpReport> FollowupReportRepository => _followupReportRepository ?? new Repository<FollowUpReport>(_dbContext);
        public IRepository<FollowUpDetail> FollowupDetailRepository => _followupDetailRepository ?? new Repository<FollowUpDetail>(_dbContext);

        public IRepository<OutreachReport> OutreachReportRepository => _outreachReportRepository ?? new Repository<OutreachReport>(_dbContext);

        public IRepository<OutreachDetail> OutreachDetailRepository => _outreachDetailRepository ?? new Repository<OutreachDetail>(_dbContext);

        public void BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _dbContext.SaveChanges();
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}