using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskFlow.Data;
using TaskFlow.DTOs.Reports;
using TaskFlow.Interfaces.Services;

namespace TaskFlow.Services
{
    public class ReportService : IReportService
    {
        private readonly TaskFlowDbContext _context;

        public ReportService(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PendingTaskReportDto>>
            GetPendingTasksAsync()
        {
            var result = new List<PendingTaskReportDto>();

            var connection = _context.Database.GetDbConnection();

            bool shouldCloseConnection =
                connection.State != ConnectionState.Open;

            if (shouldCloseConnection)
            {
                await connection.OpenAsync();
            }

            try
            {
                await using var command = connection.CreateCommand();

                command.CommandText = "TF.sp_GetPendingTasks";
                command.CommandType = CommandType.StoredProcedure;

                await using var reader =
                    await command.ExecuteReaderAsync();

                int userOrdinal =
                    reader.GetOrdinal("Usuario");

                int pendingOrdinal =
                    reader.GetOrdinal("TotalPendientes");

                int overdueOrdinal =
                    reader.GetOrdinal("TotalVencidas");

                while (await reader.ReadAsync())
                {
                    result.Add(new PendingTaskReportDto
                    {
                        Usuario = reader.GetString(userOrdinal),
                        TotalPendientes = reader.GetInt32(pendingOrdinal),
                        TotalVencidas = reader.GetInt32(overdueOrdinal)
                    });
                }
            }
            finally
            {
                if (shouldCloseConnection)
                {
                    await connection.CloseAsync();
                }
            }

            return result;
        }
    }
}
