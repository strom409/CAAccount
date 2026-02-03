using BlazorDemo.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BlazorDemo.CustomAttributes
{
    //public class UniqueSeriesNumberAttribute:ValidationAttribute
    //{

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        if (value == null)
    //            return new ValidationResult("Series number is required.");

    //        var series = (int)value;

    //        // Resolve DbContext from DI
    //        var dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext));

    //        if (dbContext == null)
    //            throw new InvalidOperationException("DbContext is not available in validation context.");

    //        bool exists = dbContext.Allocations.Any(a => a.Series == series);

    //        if (exists)
    //            return new ValidationResult("Series number already exists in Allocations table.");

    //        return ValidationResult.Success;
    //    }
    //}

    //public class UniqueSeriesNumberAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        if (value == null)
    //            return ValidationResult.Success; // let [Required]/[Range] handle it

    //        var series = (int)value;

    //        // Get the whole object (e.g., your ViewModel or Entity)
    //        var instance = validationContext.ObjectInstance;
    //        var chamberIdProp = instance.GetType().GetProperty("ChamberId");
    //        if (chamberIdProp == null)
    //            throw new InvalidOperationException("ChamberId property not found on model.");

    //        int chamberId = (int)chamberIdProp.GetValue(instance);

    //        var dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
    //        if (dbContext == null)
    //            throw new InvalidOperationException("DbContext is not available in validation context.");

    //        bool exists = dbContext.Allocations
    //            .Any(a => a.MixChamberId == chamberId && a.Series == series);

    //        if (exists)
    //            return new ValidationResult($"Series number {series} already exists in Chamber {chamberId}.");

    //        return ValidationResult.Success;
    //    }
    //}
    public class UniqueSeriesNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // let [Required]/[Range] handle this

            var series = (int)value;

            // Get the whole object (model instance)
            var instance = validationContext.ObjectInstance;

            // Expect model has Id and ChamberId
            var idProp = instance.GetType().GetProperty("Id");
            var chamberIdProp = instance.GetType().GetProperty("ChamberId");

            if (chamberIdProp == null)
                throw new InvalidOperationException("ChamberId property not found on model.");
            if (idProp == null)
                throw new InvalidOperationException("Id property not found on model.");

            int id = (int)idProp.GetValue(instance);
            int chamberId = (int)chamberIdProp.GetValue(instance);

            var dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            if (dbContext == null)
                throw new InvalidOperationException("DbContext is not available in validation context.");

            bool exists = dbContext.Allocation
                .Any(a => a.chamberid == chamberId
                       && a.Series == series
                       && a.Id != id);  // exclude current record

            if (exists)
                return new ValidationResult($"Series number {series} already exists in Chamber {chamberId}.");

            return ValidationResult.Success;
        }
    }


}
