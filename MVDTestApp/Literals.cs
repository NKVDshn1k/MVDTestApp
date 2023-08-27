
namespace MVDTestApp;

internal static class Literals
{
    internal static readonly string NotAllTasksMayBeComplited =
        "Отклонено - Не все подзадачи могут быть завершины";

    internal static readonly string CanNotDeliteNotTerminalTask =
        "Отклонено - Нельзя удалить задачу, имеющую подзадачи";

    internal static readonly string CanNotAddSubTaskToComplited =
        "Отклонено - Нельзя добавить подзадачу к выполненной задаче";

    internal static readonly string TaskNotFound =
        "Задача нет в базе данных";

    internal static readonly string Registraited =
        "Зарегистрирована";

    internal static readonly string RegistraitedAndComplited =
        "Зарегистрирована - Выполнена";

    internal static readonly string WrongParams =
        "Некорректные параметры";

    internal static readonly string CriticalError =
        "Критическая ошибка";

    internal static readonly string ResetStateError =
        "Ошибка изменения состояния задачи";

    internal static readonly string EditeError =
        "Ошибка изменения задачи";

    internal static readonly string AddError =
        "Ошибка добавления задачи";

    internal static readonly string DeleteError =
        "Ошибка удаления задачи";
    internal static readonly string LoadError =
        "Ошибка загрузки данных";

    internal static readonly string RequireFields =
        "Поля \"Имя\" и \"Исполнители\" должны быть заполнены \n+ \"Плановая трудоёмкость\" должна быть больше нуля";

    internal static readonly string FactualHoursMustBeMoreThanZero =
        "\"Фактическое время выполнения\" Должно быть больше нуля";
}
