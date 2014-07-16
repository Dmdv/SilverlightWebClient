using System;
using WebClient.ICS.Client.ViewModel;

namespace WebClient.ICS.Client.Model
{
    /// <summary>
    /// Создает информацию о валидаторах. Всего два валидатора.
    /// </summary>
    public static class ConstraintInfoCreator
    {
        /// <summary>
        /// Создает первый валидатор.
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static ConstraintInfo FirstConstraint(DataTypeEnum? dataType)
        {
            switch (dataType)
            {
                case DataTypeEnum.Double:
                case DataTypeEnum.Long:
                    return new ConstraintInfo {Name = "Min", EditType = EditType.Text};
                case DataTypeEnum.String:
                case DataTypeEnum.Xml:
                case DataTypeEnum.Bin:
                    return new ConstraintInfo {Name = "Max length", EditType = EditType.Text};
            }

            throw new ArgumentException("unexpected name");
        }

        /// <summary>
        /// Создает второй валидатор.
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static ConstraintInfo SecondConstraint(DataTypeEnum? dataType)
        {
            switch (dataType)
            {
                case DataTypeEnum.Double:
                case DataTypeEnum.Long:
                    return new ConstraintInfo {Name = "Max", EditType = EditType.Text};
                case DataTypeEnum.String:
                    return new ConstraintInfo {Name = "Regex", EditType = EditType.Text};
                case DataTypeEnum.Xml:
                    return new ConstraintInfo {Name = "Schema", EditType = EditType.XmlFileLoader};
            }

            throw new ArgumentException("unexpected name");
        }
    }
}