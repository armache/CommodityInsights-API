# Commodity Insights API

## Project Description

This is the backend project for [Commodity Insights](https://commodity-insights.azurewebsites.net/) application.

On start up it processes Excel file using open source SpreadsheetLight library, then via the code first approach creates and populates the database in SQL Server.

The generated database has 3 tables:

- Models
- Commodities
- TradeLogs

The combination of model and commodity is considered as a product. Based on provided Excel file there would be overall 3 products:

- Model1_Commodity1
- Model1_Commodity2
- Model2_Commodity2

TradeLogs table contains actual PnL data for all products.

Products and TradeLogs are exposed to other systems via API endpoints:

- /api/products
- /api/trade-logs

API link: https://angloamericanapi.azurewebsites.net/

Frontend source code: https://github.com/armache/commodity-insights

## Built With

- .NET 6
- Entity Framework Core
- Sql Server

## Testing technologies

- Sqllite (in-memory)
- XUnit
- Moq

## Points of interest
###### CommodityDbContext as DGML

![alt text](https://raw.githubusercontent.com/armache/CommodityInsights-API/master/CommodityDbContext.png)
