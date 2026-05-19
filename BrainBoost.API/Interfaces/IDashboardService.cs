using BrainBoost.API.DTOs.Dashboard;

namespace BrainBoost.API.Interfaces;

public interface IDashboardService
{
    Task<StudentDashboardDto>
        GetStudentDashboardAsync(int userId);
    Task<TeacherDashboardDto> 
        GetTeacherDashboardAsync(int userId);

    Task<AdminDashboardDto> 
        GetAdminDashboardAsync();
}