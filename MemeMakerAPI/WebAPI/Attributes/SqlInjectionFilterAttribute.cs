using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class SqlInjectionFilterAttribute : ValidationAttribute
    {
        private List<string> forbiddenPatterns;

        public SqlInjectionFilterAttribute()
        {
            forbiddenPatterns = new List<string> {
                                    @"select.+from ",
                                    @"update.+set",
                                    @"delete.+from ",
                                    @"insert\s+into",
                                    @"drop\s+(database|table)",
                                    @"or.+=.+",
                                    @"[""'`]" };
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                string input = value?.ToString();

                if (input != null)
                {
                    input = input.ToLower();
                    foreach (var expression in forbiddenPatterns)
                    {
                        Regex regex = new Regex(expression);
                        if (regex.IsMatch(input))
                        {
                            Log.Warning("Possible SQL Injection attack prevented");
                            return new ValidationResult("Input contains forbidden pattern");
                        }
                    }
                }
                return ValidationResult.Success;
            }
            catch
            {
                return new ValidationResult("Validation caused exception");
            }
        }
    }
}
