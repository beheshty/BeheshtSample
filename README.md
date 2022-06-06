# BeheshtSample
A Simple Catalog Web API
* .NET 5
* Unit Test with XUnit
________________________________

You can use the below input in all *GET* methods to filter the result

```?pageSize=10&search=your search text&searchType=1&pageNumber=1&sortColumnName=column name to sort&sortType=-1&columnfilters[0].search=your column search&columnfilters[0].columnname=name of column&columnfilters[0].searchtype=2```

all parameters are optional, default parameters are as the following:
- page size : 10
- page number : 1

values for searchType:
- searchType=1 : execute a like search on all columns (or on specific column if use in column filters)
- searchType=2 : execute an exact search on all columns (or on specific column if use in column filters)
