﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Lyu.Scaffolding.Utils;

namespace Lyu.Scaffolding.Models
{
    [Serializable]
    public class MetaColumnInfo
    {
        public string ShortTypeName { get; set; }
        public string strDataType { get; set; }
        public euColumnType DataType { get; set; }
        public euControlType ControlType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Nullable { get; set; }
        public bool Required { get; set; }
        public int? MaxLength { get; set; }
        public int? RangeMin { get; set; }
        public int? RangeMax { get; set; }

        public bool IsDtoVisible { get; set; }
        public bool IsItemVisible { get; set; }

        public bool IsNumeric
        {
            get { return ((this.DataType & euColumnType.intCT) == euColumnType.intCT); }
        }

        public bool HasMetaAttribute
        {
            get
            {
                return (MetaAttribute != string.Empty);
            }
        }

        public string MetaAttribute
        {
            get
            {
                switch (this.DataType)
                {
                    case euColumnType.stringCT:
                        //if (this.MaxLength > 0)
                        if (this.MaxLength.HasValue)
                            return string.Format("[MaxLength({0})]", this.MaxLength);
                        else
                            break;
                    case euColumnType.longCT:
                    case euColumnType.intCT:
                    case euColumnType.decimalCT:
                    case euColumnType.floatCT:
                    case euColumnType.doubleCT:
                        //if (this.RangeMin > 0 || this.RangeMax > 0)
                        if (this.RangeMin.HasValue || this.RangeMax.HasValue)
                            return string.Format("[Range({0}, {1})]", this.RangeMin, this.RangeMax);
                        else
                            break;
                    default:
                        break;
                }
                return string.Empty;
            }
        }


        //public MetaColumnInfo() { }

        //public MetaColumnInfo(Microsoft.AspNet.Scaffolding.Core.Metadata.PropertyMetadata property)
        //    : this(property.PropertyName, property.PropertyName, property.ShortTypeName, (property.RelatedModel != null))
        //{
        //}

        //public MetaColumnInfo(CodeParameter property)
        //    : this(property.Name, property.DocComment, property.Type.AsString, false)
        //{
        //}

        //public MetaColumnInfo(CodeProperty property)
        //    : this(property.Name, property.DocComment, property.Type.AsString, false)
        //{
        //}

        //private MetaColumnInfo(string strName, string strDisplayName, string strType, bool relatedModel)
        //{
        //    this.Name = strName;
        //    this.ShortTypeName = strType;
        //    this.strDataType = strType.Replace("?", "").Replace("System.", "").ToLower();

        //    if (!relatedModel)
        //    {
        //        this.DataType = GetColumnType(this.strDataType);
        //        IsVisible = true;
        //    }
        //    else
        //    {
        //        this.DataType = euColumnType.RelatedModel;
        //        IsVisible = false;  //不勾选导航属性
        //    }

        //    DisplayName = strDisplayName ?? this.Name;
        //    Nullable = true;
        //}


        public MetaColumnInfo(PropertyInfo property)
        {

            var aliase = VmUtils.ToGenericTypeString(property.PropertyType);
            var descAttributes = property.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            var strDisplayName = descAttributes.Length == 1 ? ((DisplayNameAttribute)descAttributes[0]).DisplayName : aliase;

            this.Name = property.Name;
            this.ShortTypeName = property.PropertyType.FullName;
            //this.strDataType = strType.Replace("?", "").Replace("System.", "").ToLower();
            //this.strDataType = strType.Replace("System.", "");
            this.strDataType = aliase;
            this.DataType = GetColumnType(aliase);
            DisplayName = strDisplayName ?? this.Name;
            Nullable = true;
            Required = false;
            setPropWithAttributes(property);
            if (strDataType.ToLower() == "int" || strDataType.ToLower() == "guid")
            {
                Nullable = false;
            }
            this.ControlType = GetControlType();  //放在setPropWithAttributes之后

            IsDtoVisible = IsDtoVisibleMember();
            IsItemVisible = IsItemVisibleMember();
        }

        private bool IsDtoVisibleMember()
        {
            if (DataType == euColumnType.RelatedModel) return false;
            //if (VmUtils.DtoUnSelectFields.Contains(Name)) return false;
            return true;
        }

        private bool IsItemVisibleMember()
        {
            if (DataType == euColumnType.RelatedModel) return false;
            //if (VmUtils.ItemUnSelectFields.Contains(Name)) return false;
            //if (DataType == euColumnType.stringCT && !MaxLength.HasValue) return false; //没有字符数限制的string
            return true;
        }

        private void setPropWithAttributes(PropertyInfo property)
        {
            var requiredAttributes = property.GetCustomAttributes(typeof(RequiredAttribute), true);
            if (requiredAttributes.Any())
            {
                this.Required = true;
                this.Nullable = false;
            }

            var maxLengthAttributes = property.GetCustomAttributes(typeof(MaxLengthAttribute), true);
            if (maxLengthAttributes.Any())
            {
                this.MaxLength = ((MaxLengthAttribute)maxLengthAttributes[0]).Length;
            }

            var rangeAttributes = property.GetCustomAttributes(typeof(RangeAttribute), true);
            if (rangeAttributes.Any())
            {
                var rangeAtt = ((RangeAttribute) rangeAttributes[0]);
                this.RangeMin = (int)rangeAtt.Minimum;
                this.RangeMax = (int)rangeAtt.Maximum;
            }
        }

        public euColumnType GetColumnType(string shortTypeName)
        {
            return ParseEnum(shortTypeName);
        }

        public euControlType GetControlType()
        {
            if (Name.EndsWith("Id") && Name != "Id")
                return euControlType.DropdownList;

            if (Name == "Summary" || Name == "Remark")
                return euControlType.Textarea;

            if (Name == "Content" || Name == "Body")
                return euControlType.TextEditor;

            if (DataType == euColumnType.boolCT)
                return euControlType.Checkbox;

            if (DataType == euColumnType.datetimeCT)
                return euControlType.DateTimePicker;

            if (DataType == euColumnType.stringCT && !MaxLength.HasValue)
                return euControlType.Text;

            return euControlType.Text;
        }

        //private static T ParseEnum<T>(string value)
        //{
        //    value = value+ "CT";
        //    return (T)Enum.Parse(typeof(T), value, true);
        //}

        //private static T ParseEnum2<T>(string value)
        //{
        //    value = value + "CT";
        //    T result;
        //    if (Enum.TryParse<T>(value, true, out result))
        //    {
        //        return result;
        //    }
        //    else
        //        return default(T);
        //}

        private static euColumnType ParseEnum(string value)
        {
            value = value.ToLower() + "CT";
            euColumnType result;
            if (Enum.TryParse<euColumnType>(value, true, out result))
            {
                return result;
            }
            else
                return euColumnType.RelatedModel;
        }
    }
}
