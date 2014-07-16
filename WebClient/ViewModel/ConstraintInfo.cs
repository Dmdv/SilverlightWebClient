using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.ViewModel
{
    /// <summary>
    /// Информация о Constraint.
    /// </summary>
    public class ConstraintInfo
    {
        /// <summary>
        /// Тип редактора.
        /// </summary>
        public EditType EditType { get; set; }

        /// <summary>
        /// Имя валидатора.
        /// </summary>
        public string Name { get; set; }
    }
}