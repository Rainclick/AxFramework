using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Common.Exception;
using ExpressionBuilder.Common;
using ExpressionBuilder.Generics;
using ExpressionBuilder.Interfaces;
using ExpressionBuilder.Operations;

namespace Common
{

    public class DataRequest
    {
        //string filter, string sort, int page, int PageSize, SortType SortType
        public List<AxFilter> Filters { get; set; }
        public string Sort { get; set; }
        public int PageSize { get; set; }
        public SortType SortType { get; set; }
        public int PageIndex { get; set; }
    }

    public class AxFilter
    {
        public string Property { get; set; }
        public OperationType Operation { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public Connector Connector { get; set; }
    }

    public enum OperationType
    {
        Between = 1,
        Contains = 2,
        DoesNotContain = 3,
        EndsWith = 4,
        EqualTo = 5,
        GreaterThan = 6,
        GreaterThanOrEqualTo = 7,
        In = 8,
        NotIn = 9,
        IsNull = 10,
        IsEmpty = 11,
        IsNotEmpty = 12,
        IsNotNull = 13,
        IsNotNullNorWhiteSpace = 14,
        LessThan = 15,
        LessThanOrEqualTo = 16,
        NotEqualTo = 17,
        StartsWith = 18
    }
    public static class OperationExt
    {
        public static IOperation GetOperation(this OperationType type)
        {
            switch (type)
            {
                case OperationType.Between:
                    return Operation.Between;
                case OperationType.Contains:
                    return Operation.Contains;
                case OperationType.DoesNotContain:
                    return Operation.DoesNotContain;
                case OperationType.EndsWith:
                    return Operation.EndsWith;
                case OperationType.EqualTo:
                    return Operation.EqualTo;
                case OperationType.GreaterThan:
                    return Operation.GreaterThan;
                case OperationType.GreaterThanOrEqualTo:
                    return Operation.GreaterThanOrEqualTo;
                case OperationType.In:
                    return Operation.In;
                case OperationType.NotIn:
                    return Operation.NotIn;
                case OperationType.IsNull:
                    return Operation.IsNull;
                case OperationType.IsEmpty:
                    return Operation.IsEmpty;
                case OperationType.IsNotEmpty:
                    return Operation.IsNotEmpty;
                case OperationType.IsNotNull:
                    return Operation.IsNotNull;
                case OperationType.IsNotNullNorWhiteSpace:
                    return Operation.IsNotNullNorWhiteSpace;
                case OperationType.LessThan:
                    return Operation.LessThan;
                case OperationType.LessThanOrEqualTo:
                    return Operation.LessThanOrEqualTo;
                case OperationType.NotEqualTo:
                    return Operation.NotEqualTo;
                case OperationType.StartsWith:
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static Filter<TClass> GetFilter<TClass>(this DataRequest request) where TClass : class
        {
            if (request.Filters != null)
            {
                var predicate = new Filter<TClass>();
                foreach (var requestFilter in request.Filters)
                {
                    var entityType = typeof(TClass);
                    var property = entityType.GetProperties().FirstOrDefault(x => x.Name.Equals(requestFilter.Property, StringComparison.InvariantCultureIgnoreCase));
                    if (property == null)
                        throw new AppException(ApiResultStatusCode.BadRequest, $"{requestFilter.Property} not found in {entityType.Name}");

                    var type = Nullable.GetUnderlyingType(property.GetType()) ?? property.PropertyType;
                    var typeConverter = TypeDescriptor.GetConverter(type);
                    var value1 = typeConverter.ConvertFromString(requestFilter.Value1);
                    object value2 = null;
                    if (!string.IsNullOrWhiteSpace(requestFilter.Value2))
                        value2 = typeConverter.ConvertFromString(requestFilter.Value2);

                    predicate.By(requestFilter.Property, requestFilter.Operation.GetOperation(), value1, value2, requestFilter.Connector);
                }
                return predicate;
            }

            return null;
        }

        public static bool HasMoreThanThreeElements<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Count() > 3;
        }
    }
}
