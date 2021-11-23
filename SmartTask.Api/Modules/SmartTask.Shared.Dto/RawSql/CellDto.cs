using System;
namespace SmartTask.Shared.DTO.RawSql
{
    public class CellDto
    {
        public string Column { get; set; }

        public object Value { get; set; }

        public object OldValue { get; set; }

        public string FieldType { get; set; }

        public bool IsChanged { get; set; }

        public CellDto(string column, object value, Type fieldType = null)
        {
            Column = column;
            Value = value;
            FieldType = (fieldType ?? typeof(object)).Name;
        }
    }
}
