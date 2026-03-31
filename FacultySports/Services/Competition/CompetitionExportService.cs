using ClosedXML.Excel;
using FacultySports.Domain.Entities;
using FacultySports.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FacultySports.MVC.Services.Competitions;

public class CompetitionExportService : IExportService<Competition>
{
    private const string RootWorksheetName = "Competitions";
    private static readonly IReadOnlyList<string> HeaderNames = new[]
    {
        "Title",
        "Section",
        "Date",
        "StartTime",
        "Status",
        "Location",
    };

    private readonly SportsDbContext context;

    public CompetitionExportService(SportsDbContext context)
    {
        this.context = context;
    }

    public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (!stream.CanWrite)
        {
            throw new ArgumentException("Output stream is not writable.", nameof(stream));
        }

        var competitions = await context.Competitions
            .Include(c => c.Section)
            .Include(c => c.Status)
            .Include(c => c.Location)
            .ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(RootWorksheetName);
        WriteHeader(worksheet);
        WriteCompetitions(worksheet, competitions);
        workbook.SaveAs(stream);
    }

    private static void WriteHeader(IXLWorksheet worksheet)
    {
        for (var columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
        {
            worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
        }

        worksheet.Row(1).Style.Font.Bold = true;
    }

    private static void WriteCompetition(IXLWorksheet worksheet, Competition competition, int rowIndex)
    {
        var columnIndex = 1;
        worksheet.Cell(rowIndex, columnIndex++).Value = competition.Title;
        worksheet.Cell(rowIndex, columnIndex++).Value = competition.Section?.Name ?? string.Empty;
        worksheet.Cell(rowIndex, columnIndex++).Value = competition.Date.ToString("yyyy-MM-dd");
        worksheet.Cell(rowIndex, columnIndex++).Value = competition.StartTime.ToString("HH:mm");
        worksheet.Cell(rowIndex, columnIndex++).Value = competition.Status?.Name ?? string.Empty;
        worksheet.Cell(rowIndex, columnIndex++).Value = competition.Location?.Name ?? string.Empty;
    }

    private static void WriteCompetitions(IXLWorksheet worksheet, IReadOnlyCollection<Competition> competitions)
    {
        var rowIndex = 2;
        foreach (var competition in competitions)
        {
            WriteCompetition(worksheet, competition, rowIndex);
            rowIndex++;
        }
    }
}
