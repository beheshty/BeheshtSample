# BeheshtSample
A Simple Catalog Web API
* .NET 5
* Unit Test with XUnit
________________________________

You can use the below input in all *GET* methods to filter the result

```querystring
?pageSize=10&search=your search text&searchType=1&pageNumber=1&sortColumnName=column name to sort&sortType=-1&columnfilters[0].search=your column search&columnfilters[0].columnname=name of column&columnfilters[0].searchtype=2
```

all parameters are optional, default parameters are as the following:
- page size: 10
- page number: 1

values for searchType:
- searchType=1: execute a like search on all columns (or on a specific column if used in column filters)
- searchType=2: execute an exact search on all columns (or on a specific column if used in column filters)

_______________________________

All methods will generate the same output:
```json
{
  "success": true,
  "message": null,
  "data": ["object or array"],
  "status": 200
}
```
Or if you are using any get methods:
```json
{
  "data": ["Array of objects"],
  "success": true,
  "message": "",
  "status": 200,
  "metaData": {
    "search": "",
    "pageNumber": 0,
    "pageSize": 0,
    "totalItemCount": 0,
    "searchType": 0,
    "isFirstPage": false,
    "isLastPage": false,
    "hasPreviousPage": false,
    "hasNextPage": false,
    "pageCount": 0,
    "nextPageNumber": 0,
    "previousPageNumber": 0,
    "columnFilters": [
    {
      "columnName": null,
      "search": null,
      "searchType": 0
    }],
    "columnSorting": {
      "columnName": null,
      "type": 0
    }
  }
}
```
