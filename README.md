# Excel client for Materialize using ExcelDNA


## Materialize demo set up

https://github.com/MaterializeInc/demos/tree/main/ecommerce-redpanda

## Known limitations

Only works with native Excel 365 on Windows.
- Excel for Mac does not support the native C API that Excel-DNA uses).
- Older versions of Excel don't support dynamic arrays.

Order by clauses are not implemented.

Large grids are currently inefficient due to copy from multiset to 2d array.