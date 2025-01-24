using AttendanceSystem.Application.Features.Fellowships.Commands.AddFellowship;
using AttendanceSystem.Application.Features.Fellowships.Commands.EditFellowship;
using AttendanceSystem.Application.Features.Fellowships.Queries.GetAllFellowships;
using AttendanceSystem.Application.Features.Fellowships.Queries.GetFellowship;
using AttendanceSystem.Application.Features.Members.Commands.AddMember;
using AttendanceSystem.Application.Features.Members.Commands.EditMember;
using AttendanceSystem.Application.Features.Members.Queries.GetAllMembers;
using AttendanceSystem.Application.Features.Members.Queries.GetMember;
using AttendanceSystem.Application.Features.Pastors.Commands.AddPastor;
using AttendanceSystem.Application.Features.Pastors.Commands.EditPastor;
using AttendanceSystem.Application.Features.Pastors.Queries.GetAllPastors;
using AttendanceSystem.Application.Features.Pastors.Queries.GetPastor;
using AttendanceSystem.Application.Features.Reports.Attendance.Commands.Create;
using AttendanceSystem.Application.Features.Reports.Attendance.Commands.Edit;
using AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetAll;
using AttendanceSystem.Application.Features.Reports.Attendance.Queries.GetSingle;
using AttendanceSystem.Application.Features.Reports.Followup.Commands.Create;
using AttendanceSystem.Application.Features.Reports.Followup.Commands.Edit;
using AttendanceSystem.Application.Features.Reports.Followup.Queries.GetAll;
using AttendanceSystem.Application.Features.Reports.Followup.Queries.GetSingle;
using AttendanceSystem.Application.Features.Reports.Outreach.Commands.Create;
using AttendanceSystem.Application.Features.Reports.Outreach.Commands.Edit;
using AttendanceSystem.Application.Features.Reports.Outreach.Queries.GetAll;
using AttendanceSystem.Application.Features.Reports.Outreach.Queries.GetSingle;
using AttendanceSystem.Application.Features.Setups.Activities.Queries.GetActivity;
using AttendanceSystem.Application.Features.Setups.Activities.Queries.GetAllActivities;
using AttendanceSystem.Domain.Entities;
using AutoMapper;

namespace AttendanceSystem.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Activity, ActivityDetailResultVM>();

            CreateMap<Activity, ActivitiesListResultVM>();

            CreateMap<PagedResult<Activity>, PagedResult<ActivitiesListResultVM>>();

            /* Start Member Mapping */

            CreateMap<AddMemberCommand, Member>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.LastLoginDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Fellowship, opt => opt.Ignore())
                .ForMember(dest => dest.LoginAttempt, opt => opt.Ignore())
                .ForMember(dest => dest.LoginAccessDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsPasswordLocked, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<EditMemberCommand, Member>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(b => b.MemberId))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.LastLoginDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Fellowship, opt => opt.Ignore())
                .ForMember(dest => dest.LoginAttempt, opt => opt.Ignore())
                .ForMember(dest => dest.LoginAccessDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsPasswordLocked, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            /*CreateMap<PagedResult<Member>, GetAllMembersQueryResponse>()
               .ForMember(dest => dest.Result, opt => opt.MapFrom(b => b))
               .ForMember(dest => dest.Success, opt => opt.Ignore())
               .ForMember(dest => dest.Message, opt => opt.Ignore())
               .ForMember(dest => dest.ValidationErrors, opt => opt.Ignore());


            CreateMap<PagedResult<Member>, GetMemberQueryResponse>()
               .ForMember(dest => dest.Result, opt => opt.MapFrom(b => b))
               .ForMember(dest => dest.Success, opt => opt.Ignore())
               .ForMember(dest => dest.Message, opt => opt.Ignore())
               .ForMember(dest => dest.ValidationErrors, opt => opt.Ignore());*/

            CreateMap<Member, MemberDetailResultVM>()
                .ForMember(dest => dest.FellowshipName, opt => opt.MapFrom(i => i.Fellowship.Name))
                .ForMember(dest => dest.DisciplerFullName, opt => opt.Ignore());

            CreateMap<Member, MembersListResultVM>()
                .ForMember(dest => dest.FellowshipName, opt => opt.MapFrom(i => i.Fellowship.Name))
                .ForMember(dest => dest.DisciplerFullName, opt => opt.Ignore());

            CreateMap<PagedResult<Member>, PagedResult<MembersListResultVM>>();

            /* End Member Mapping */

            /* Start Attendance Report Mapping */

            CreateMap<CreateAttendanceCommand, AttendanceReport>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Member, opt => opt.Ignore())
                .ForMember(dest => dest.Activity, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<EditAttendanceCommand, AttendanceReport>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(b => b.ReportId))
                .ForMember(dest => dest.Member, opt => opt.Ignore())
                .ForMember(dest => dest.Activity, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            /*CreateMap<PagedResult<AttendanceReport>, GetAttendanceReportsQueryResponse>()
               .ForMember(dest => dest.Result, opt => opt.MapFrom(b => b))
               .ForMember(dest => dest.Success, opt => opt.Ignore())
               .ForMember(dest => dest.Message, opt => opt.Ignore())
               .ForMember(dest => dest.ValidationErrors, opt => opt.Ignore());

            CreateMap<PagedResult<AttendanceReport>, GetAttendanceReportQueryResponse>()
               .ForMember(dest => dest.Result, opt => opt.MapFrom(b => b))
               .ForMember(dest => dest.Success, opt => opt.Ignore())
               .ForMember(dest => dest.Message, opt => opt.Ignore())
               .ForMember(dest => dest.ValidationErrors, opt => opt.Ignore());*/

            CreateMap<AttendanceReport, AttendanceReportDetailResultVM>()
                .ForMember(dest => dest.MemberFullName, opt => opt.MapFrom(i => string.Join(' ', i.Member.FirstName, i.Member.LastName)))
                .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(i => i.Activity.Name));
            CreateMap<AttendanceReport, AttendanceReportListResultVM>()
                .ForMember(dest => dest.MemberFullName, opt => opt.MapFrom(i => string.Join(' ', i.Member.FirstName, i.Member.LastName)))
                .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(i => i.Activity.Name));

            CreateMap<PagedResult<AttendanceReport>, PagedResult<AttendanceReportListResultVM>>();

            /* End Attendance Report Mapping */

            /* Start Follow-up Report Mapping */

            CreateMap<CreateFollowupReportCommand, FollowUpReport>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Member, opt => opt.Ignore())
                .ForMember(dest => dest.Activity, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<CreateFollowUpDetailCommand, FollowUpDetail>()
                .ForMember(dest => dest.FollowUpReportId, opt => opt.Ignore())
                .ForMember(dest => dest.FollowUpReport, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<EditFollowupReportCommand, FollowUpReport>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(b => b.ReportId))
                .ForMember(dest => dest.Member, opt => opt.Ignore())
                .ForMember(dest => dest.Activity, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<EditFollowUpDetailCommand, FollowUpDetail>()
                .ForMember(dest => dest.FollowUpReportId, opt => opt.Ignore())
                .ForMember(dest => dest.FollowUpReport, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());


            /*CreateMap<PagedResult<FollowUpReport>, GetFollowupReportsQueryResponse>()
               .ForMember(dest => dest.Result, opt => opt.MapFrom(b => b))
               .ForMember(dest => dest.Success, opt => opt.Ignore())
               .ForMember(dest => dest.Message, opt => opt.Ignore())
               .ForMember(dest => dest.ValidationErrors, opt => opt.Ignore());

            CreateMap<PagedResult<FollowUpReport>, GetFollowupReportQueryResponse>()
               .ForMember(dest => dest.Result, opt => opt.MapFrom(b => b))
               .ForMember(dest => dest.Success, opt => opt.Ignore())
               .ForMember(dest => dest.Message, opt => opt.Ignore())
               .ForMember(dest => dest.ValidationErrors, opt => opt.Ignore());*/


            CreateMap<FollowUpReport, FollowupReportDetailResultVM>()
                .ForMember(dest => dest.MemberFullName, opt => opt.MapFrom(i => string.Join(' ', i.Member.FirstName, i.Member.LastName)))
                .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(i => i.Activity.Name));
            CreateMap<FollowUpReport, FollowupReportListResultVM>()
                .ForMember(dest => dest.MemberFullName, opt => opt.MapFrom(i => string.Join(' ', i.Member.FirstName, i.Member.LastName)))
                .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(i => i.Activity.Name));

            CreateMap<PagedResult<FollowUpReport>, PagedResult<FollowupReportListResultVM>>();

            CreateMap<FollowUpDetail, FollowUpDetailResultVM>();

            CreateMap<PagedResult<FollowUpDetail>, PagedResult<FollowUpDetailResultVM>>();

            /* End Follow-up Report Mapping */

            /* Start Outreach Report Mapping */

            CreateMap<CreateOutreachReportCommand, OutreachReport>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Member, opt => opt.Ignore())
                .ForMember(dest => dest.Activity, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<CreateOutreachDetailCommand, OutreachDetail>()
                .ForMember(dest => dest.OutreachReportId, opt => opt.Ignore())
                .ForMember(dest => dest.OutreachReport, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<EditOutreachReportCommand, OutreachReport>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(b => b.ReportId))
                .ForMember(dest => dest.Member, opt => opt.Ignore())
                .ForMember(dest => dest.Activity, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<EditOutreachDetailCommand, OutreachDetail>()
                .ForMember(dest => dest.OutreachReportId, opt => opt.Ignore())
                .ForMember(dest => dest.OutreachReport, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<OutreachReport, OutreachReportDetailResultVM>()
                .ForMember(dest => dest.MemberFullName, opt => opt.MapFrom(i => string.Join(' ', i.Member.FirstName, i.Member.LastName)))
                .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(i => i.Activity.Name));
            CreateMap<OutreachReport, OutreachReportListResultVM>()
                .ForMember(dest => dest.MemberFullName, opt => opt.MapFrom(i => string.Join(' ', i.Member.FirstName, i.Member.LastName)))
                .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(i => i.Activity.Name));

            CreateMap<PagedResult<OutreachReport>, PagedResult<OutreachReportListResultVM>>();

            CreateMap<OutreachDetail, OutreachDetailResultVM>();

            CreateMap<PagedResult<OutreachDetail>, PagedResult<OutreachDetailResultVM>>();

            /* End Outreach Report Mapping */

            /* Start Pastor Mapping */

            CreateMap<AddPastorCommand, Pastor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.LastLoginDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Fellowship, opt => opt.Ignore())
                .ForMember(dest => dest.LoginAttempt, opt => opt.Ignore())
                .ForMember(dest => dest.LoginAccessDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsPasswordLocked, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<EditPastorCommand, Pastor>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(b => b.PastorId))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.LastLoginDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Fellowship, opt => opt.Ignore())
                .ForMember(dest => dest.LoginAttempt, opt => opt.Ignore())
                .ForMember(dest => dest.LoginAccessDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsPasswordLocked, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<Pastor, PastorDetailResultVM>()
                .ForMember(dest => dest.FellowshipName, opt => opt.MapFrom(i => i.Fellowship.Name));

            CreateMap<Pastor, PastorsListResultVM>()
                .ForMember(dest => dest.FellowshipName, opt => opt.MapFrom(i => i.Fellowship.Name));

            CreateMap<PagedResult<Pastor>, PagedResult<PastorsListResultVM>>();

            /* End Pastor Mapping */

            /* Start Fellowship Mapping */

            CreateMap<AddFellowshipCommand, Fellowship>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<EditFellowshipCommand, Fellowship>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(b => b.FellowshipId))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<Fellowship, FellowshipDetailResultVM>()
                .ForMember(dest => dest.PastorFullName, opt => opt.Ignore());

            CreateMap<Fellowship, FellowshipsListResultVM>()
                .ForMember(dest => dest.PastorFullName, opt => opt.Ignore());

            CreateMap<PagedResult<Fellowship>, PagedResult<FellowshipsListResultVM>>();

            /* End Fellowship Mapping */
        }
    }
}