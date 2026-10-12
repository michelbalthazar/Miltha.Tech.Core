namespace Miltha.Tech.Core.Security
{
    public enum Roles
    {
        /// <summary>
        /// Main user, caretaker of the system.
        /// </summary>
        Root = 1,

        /// <summary>
        /// User with total access to the system.
        /// </summary>
        Admin = 2,

        /// <summary>
        /// User with read and write access.
        /// </summary>
        ReadWrite = 3,

        /// <summary>
        /// User with read-only access.
        /// </summary>
        ReadOnly = 4,

        /// <summary>
        /// User with limited access.
        /// </summary>
        Visitor = 5,
    }
}