using MVDTestApp.Model;

namespace MVDTestApp.Locator
{
    internal class TaskDataPackage
    {
        public readonly bool IsEdite;
        public readonly WorkTask Task;
        public readonly WorkTask Parent;

        public TaskDataPackage(bool isEdite, WorkTask task, WorkTask parent = null)
        {
            IsEdite = isEdite;
            Task = task;
            Parent = parent;
        }

    }
}
