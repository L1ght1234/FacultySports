namespace FacultySports.Application.Constants;

public static class ErrorMessagesConstants
{
    public static string NotFound()
    {
        return "Not Found";
    }
    public static string NotFound(object? id, Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        return $"Entity {entityType.Name} with id '{id}' was not found";
    }
    public static string FailedToCreateEntity(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        return $"Failed to create new {entityType.Name}";
    }

    public static string FailedToCreateEntityInDatabase(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        return $"Failed to create new {entityType.Name} in the database";
    }

    public static string FailedToUpdateEntity(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        return $"Failed to update {entityType.Name}";
    }

    public static string FailedToUpdateEntityInDatabase(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        return $"Failed to update {entityType.Name} in the database";
    }

    public static string FailedToDeleteEntity(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        return $"Failed to delete {entityType.Name}";
    }

    public static string FailedToDeleteEntityInDatabase(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        return $"Failed to delete {entityType.Name} in the database";
    }
}
