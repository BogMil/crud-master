﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;

namespace CrudMaster
{
    public interface IMappingService
    {
        TDestination Map<TSource, TDestination>(TSource source);
        IMapper Mapper { get; set; }
        TypeMap GetTypeMapFor(Type sourceType, Type destinationType);

        LambdaExpression GetMappingExpressionFromDestinationPropToSourceProp(string destinationPropertyName,
            Type destinationType, Type sourceType);

        string GetPropertyPathInSourceType(string destinationPropertyName, Type destinationType, Type sourceType);

        string GetFkNameInSourceForDestinationFkName(string destinationFkName, Type destinationType, Type sourceType);
    }

    public class MappingService : IMappingService
    {
        public IMapper Mapper { get; set; }= Mapping.Mapper;

        public TDestination Map<TSource,TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public TypeMap GetTypeMapFor(Type sourceType, Type destinationType)
        {
            return Mapper.GetTypeMapFor(sourceType, destinationType);
        }


        public LambdaExpression GetMappingExpressionFromDestinationPropToSourceProp(string destinationPropertyName, Type destinationType, Type sourceType)
        {
            var typeMap = Mapper.GetTypeMapFor(sourceType, destinationType);
           
            var propertyMap = typeMap.GetPropertyMapByDestinationPropertyName(destinationPropertyName);
            if (propertyMap.CustomMapExpression != null)
                return propertyMap.CustomMapExpression;

            var sourceTypeLambdaExpressionCreatorType = typeof(LambdaExpressionCreator<>).MakeGenericType(sourceType);
            dynamic linkedTableExpressionCreator =
                Activator.CreateInstance(sourceTypeLambdaExpressionCreatorType, destinationPropertyName);
            return linkedTableExpressionCreator.LambdaExpression;
        }

        public string GetPropertyPathInSourceType(string destinationPropertyName, Type destinationType, Type sourceType)
        {
            var mapingExpression =
                GetMappingExpressionFromDestinationPropToSourceProp(destinationPropertyName, destinationType, sourceType);
            var expressionBodyString = mapingExpression.Body.ToString();
            var firstDotIndex = expressionBodyString.IndexOf(".") + 1;
            var propertyPath = expressionBodyString.Substring(firstDotIndex, expressionBodyString.Length - firstDotIndex);
            return propertyPath;
        }

        public string GetFkNameInSourceForDestinationFkName(string destinationFkName, Type destinationType, Type sourceType)
        {
            var typeMap = GetTypeMapFor(sourceType, destinationType: destinationType);
            var fkPropertyMap = typeMap.GetPropertyMapByDestinationPropertyName(destinationFkName);
            return fkPropertyMap.GetNameOfForeignKeyInSource();
        }
    }
}