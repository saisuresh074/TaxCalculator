namespace TaxCalculator.Domain.Exceptions;

public class CountryConfigNotFoundException : Exception
{
    public string CountryCode { get; }

    public CountryConfigNotFoundException(string countryCode)
        : base($"No tax configuration found for country '{countryCode}'.")
    {
        CountryCode = countryCode;
    }
}

public class InvalidTaxConfigurationException : Exception
{
    public InvalidTaxConfigurationException(string message) : base(message) { }
}

public class NoStrategyRegisteredException : Exception
{
    public NoStrategyRegisteredException(string taxItemType)
        : base($"No calculation strategy is registered for tax item type '{taxItemType}'. " +
               "Register a strategy in ITaxCalculationStrategyRegistry.") { }
}
