# TaxCalculator
Configure and calculate tax based on the country


# Key highlights:

- Clean layered architecture (Domain, Application, Infrastructure, API)
- Strategy Pattern eliminates inheritance complexity and switch-case logic
- Easily extendable — add new tax types with zero changes to existing code
- Supports configurable tax rules per country

## API Endpoints

### POST /api/tax/configure

```json
{
  "countryCode": "DE",
  "taxItems": [
    { "name": "CommunityTax", "type": 1, "amount": 1500 },
    { "name": "RadioTax",     "type": 1, "amount": 500  },
    { "name": "PensionTax",   "type": 2, "rate": 20     },
    {
      "name": "IncomeTax", "type": 3,
      "brackets": [
        { "threshold": 10000, "rate": 0  },
        { "threshold": 30000, "rate": 20 },
        { "threshold": null,  "rate": 40 }
      ]
    }
  ]
}
```

type: 1 = Fixed · 2 = FlatRate · 3 = Progressive

### POST /api/tax/calculate

Request: `{ "countryCode": "DE", "grossSalary": 62000 }`

Response:
```json
{
  "grossSalary": 62000, "taxableBase": 60000,
  "totalTaxes": 30000,  "netSalary": 32000,
  "breakdown": [
    { "name": "CommunityTax", "type": "Fixed",       "amount": 1500  },
    { "name": "RadioTax",     "type": "Fixed",       "amount": 500   },
    { "name": "PensionTax",   "type": "FlatRate",    "amount": 12000 },
    { "name": "IncomeTax",    "type": "Progressive", "amount": 16000 }
  ]
}
```
