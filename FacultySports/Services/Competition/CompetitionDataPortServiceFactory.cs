using FacultySports.Domain.Entities;
using FacultySports.Infrastructure.Context;
using FacultySports.MVC.Services;

namespace FacultySports.MVC.Services.Competitions;

public class CompetitionDataPortServiceFactory : IDataPortServiceFactory<Competition>
{
    private const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    private readonly SportsDbContext context;

    public CompetitionDataPortServiceFactory(SportsDbContext context)
    {
        this.context = context;
    }

    public IExportService<Competition> GetExportService(string contentType)
    {
        if (string.Equals(contentType, ExcelContentType, StringComparison.OrdinalIgnoreCase))
        {
            return new CompetitionExportService(context);
        }

        throw new NotImplementedException($"No export service implemented for competitions with content type {contentType}");
    }

    public IImportService<Competition> GetImportService(string contentType)
    {
        if (string.Equals(contentType, ExcelContentType, StringComparison.OrdinalIgnoreCase))
        {
            return new CompetitionImportService(context);
        }

        throw new NotImplementedException($"No import service implemented for competitions with content type {contentType}");
    }
}
