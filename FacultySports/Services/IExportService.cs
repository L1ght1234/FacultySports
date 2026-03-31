using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FacultySports.MVC.Services;

public interface IExportService<TEntity>
    where TEntity : class
{
    Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
}
