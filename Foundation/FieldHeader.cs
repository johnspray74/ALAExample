using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation
{
    /// <summary>
    /// Defines the type of field. 
    /// </summary>
    enum FieldType
    {
        Numeric,
        Alphanumeric,
        Custom,
        Date,
        Time
    }

    /// <summary>
    /// Describes a field header from a device.
    /// All fields have a type (see FieldType), identifier (for device side) and a name (user friendly).
    /// </summary>
    class FieldHeader
    {
        public string Id { get => id; }
        public FieldType Type { get => type; }
        public string Name { get => name; }
        public bool IsLifeData { get => id.StartsWith("F"); }

        private string id;
        private FieldType type;
        private string name;

        private static readonly Dictionary<char, FieldType> fieldTypes = new Dictionary<char, FieldType>()
        {
            { 'N', FieldType.Numeric },
            { 'A', FieldType.Alphanumeric },
            { 'O', FieldType.Custom },
            { 'D', FieldType.Date },
            { 'T', FieldType.Time }
        };

        /// <summary>
        /// Describes a field header from a device.
        /// All fields have a type (see FieldType), identifier (for device side) and a name (user friendly).
        /// </summary>
        private FieldHeader(string id, FieldType type, string name)
        {
            this.id = id;
            this.type = type;
            this.name = name;
        }

        /// <summary>
        /// Converts a field stringt to a FieldHeader object.
        /// </summary>
        /// <param name="field">The field string to convert to an object.</param>
        /// <returns>A field header object.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the field string is invalid and could not be converted.</exception>
        public static FieldHeader FromString(string field)
        {
            string fieldId = field.Substring(0, 2);
            char fieldTypeC = field.ElementAt(2);
            string fieldName = field.Substring(3);

            try
            {
                FieldType fieldType = fieldTypes[fieldTypeC];

                return new FieldHeader(fieldId, fieldType, fieldName);
            }
            catch
            {
                throw new ArgumentException("Unable to parse the given field.");   
            }
        }
    }
}