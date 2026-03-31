namespace FacultySports.MVC.Services;

public interface IDataPortServiceFactory<TEntity>
    where TEntity : class
{
    IExportService<TEntity> GetExportService(string contentType);

    IImportService<TEntity> GetImportService(string contentType);
}
