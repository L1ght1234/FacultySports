using ClosedXML.Excel;
using FacultySports.Domain.Entities;
using FacultySports.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FacultySports.MVC.Services.Competitions;

public class CompetitionImportService : IImportService<Competition>
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

    public CompetitionImportService(SportsDbContext context)
    {
        this.context = context;
    }

    public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (!stream.CanRead)
        {
            throw new ArgumentException("Input stream is not readable.", nameof(stream));
        }

        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == RootWorksheetName) ?? workbook.Worksheets.First();

        if (!HasHeaderRow(worksheet.Row(1)))
        {
            throw new InvalidOperationException("The workbook must contain a header row with the expected columns.");
        }

        var rowIndex = 2;
        var addedAnyCompetition = false;

        while (true)
        {
            var row = worksheet.Row(rowIndex);
            var title = ReadCell(row, 1);
            if (string.IsNullOrWhiteSpace(title))
            {
                break;
            }

            var sectionName = ReadCell(row, 2);
            var dateCell = row.Cell(3);
            var timeCell = row.Cell(4);
            var statusName = ReadCell(row, 5);
            var locationName = ReadCell(row, 6);

            if (!TryParseDate(dateCell, out var date) || !TryParseTime(timeCell, out var startTime))
            {
                rowIndex++;
                continue;
            }

            if (await CompetitionExistsAsync(title, date, startTime, cancellationToken))
            {
                rowIndex++;
                continue;
            }

            var section = string.IsNullOrWhiteSpace(sectionName) ? null : await GetOrCreateSectionAsync(sectionName, cancellationToken);
            var status = string.IsNullOrWhiteSpace(statusName) ? null : await GetOrCreateStatusAsync(statusName, cancellationToken);
            var location = string.IsNullOrWhiteSpace(locationName) ? null : await GetOrCreateLocationAsync(locationName, cancellationToken);

            var competition = new Competition
            {
                Title = title,
                Date = date,
                StartTime = startTime,
                Section = section,
                Status = status,
                Location = location,
            };

            context.Competitions.Add(competition);
            addedAnyCompetition = true;
            rowIndex++;
        }

        if (addedAnyCompetition)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private static string ReadCell(IXLRow row, int columnIndex)
    {
        return row.Cell(columnIndex).GetValue<string>()?.Trim() ?? string.Empty;
    }

    private static bool HasHeaderRow(IXLRow row)
    {
        for (var i = 0; i < HeaderNames.Count; i++)
        {
            var headerValue = row.Cell(i + 1).GetValue<string>()?.Trim();
            if (!string.Equals(headerValue, HeaderNames[i], StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }

    private static bool TryParseDate(IXLCell cell, out DateOnly date)
    {
        date = default;

        if (cell.DataType == XLDataType.DateTime)
        {
            var dt = cell.GetDateTime();
            date = DateOnly.FromDateTime(dt);
            return true;
        }

        var raw = cell.GetValue<string>()?.Trim();
        if (DateOnly.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            return true;
        }

        if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
        {
            date = DateOnly.FromDateTime(parsedDateTime);
            return true;
        }

        return false;
    }

    private static bool TryParseTime(IXLCell cell, out TimeOnly time)
    {
        time = default;

        if (cell.DataType == XLDataType.TimeSpan)
        {
            var span = cell.GetTimeSpan();
            time = TimeOnly.FromTimeSpan(span);
            return true;
        }

        if (cell.DataType == XLDataType.DateTime)
        {
            var dt = cell.GetDateTime();
            time = TimeOnly.FromDateTime(dt);
            return true;
        }

        var raw = cell.GetValue<string>()?.Trim();
        if (TimeOnly.TryParse(raw, CultureInfo.InvariantCulture, out time))
        {
            return true;
        }

        if (TimeSpan.TryParse(raw, CultureInfo.InvariantCulture, out var parsedSpan))
        {
            time = TimeOnly.FromTimeSpan(parsedSpan);
            return true;
        }

        if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
        {
            time = TimeOnly.FromDateTime(parsedDateTime);
            return true;
        }

        return false;
    }

    private async Task<bool> CompetitionExistsAsync(string title, DateOnly date, TimeOnly startTime, CancellationToken cancellationToken)
    {
        return await context.Competitions.AnyAsync(c => c.Title == title && c.Date == date && c.StartTime == startTime, cancellationToken);
    }

    private async Task<Section?> GetOrCreateSectionAsync(string name, CancellationToken cancellationToken)
    {
        var section = await context.Sections.FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
        if (section is null)
        {
            section = new Section { Name = name };
            context.Sections.Add(section);
        }

        return section;
    }

    private async Task<CompetitionStatus?> GetOrCreateStatusAsync(string name, CancellationToken cancellationToken)
    {
        var status = await context.CompetitionStatuses.FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
        if (status is null)
        {
            status = new CompetitionStatus { Name = name, Code = name };
            context.CompetitionStatuses.Add(status);
        }

        return status;
    }

    private async Task<Location?> GetOrCreateLocationAsync(string name, CancellationToken cancellationToken)
    {
        var location = await context.Locations.FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
        if (location is null)
        {
            location = new Location { Name = name };
            context.Locations.Add(location);
        }

        return location;
    }
}
