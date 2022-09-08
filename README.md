# Excel client for Materialize using System.Reactive and Excel-DNA

[Materialize](https://materialize.com/) is an awesome streaming SQL database. 

Excel is... better at displaying real-time data than many people realize.

This add-in provides a single UDF `=MZ_TAIL(query)` where query is an object (source, table, view) name or select statement.

![MZ_TAIL usage](doc/mz_tail.gif?raw=true "MZ_TAIL")

There is also a custom task pane which displays the data catalog from the Materialize server.

![custom task page](doc/mz_task_pane.gif?raw=true "Custom task pane")

## Installation

Download the release or build the solution yourself (artifacts will be in `MaterializeExcel.AddIn\bin\Debug`).

Edit the `.config` file for your Materialize host.
```
<MaterializeExcel.AddIn.Properties.Settings>
    <setting name="Host" serializeAs="String">
        <value>localhost</value>
    </setting>
    <setting name="Port" serializeAs="String">
        <value>6875</value>
    </setting>
    <setting name="Database" serializeAs="String">
        <value>materialize</value>
    </setting>
    <setting name="User" serializeAs="String">
        <value>materialize</value>
    </setting>
</MaterializeExcel.AddIn.Properties.Settings>
```

Follow the [Add or remove an Excel add-in](https://support.microsoft.com/en-us/office/add-or-remove-add-ins-in-excel-0af570c4-5cf3-4fa9-9b88-403625a0b460) instructions from Microsoft. Make sure the `.xll` and `.config` files stay together. 

Note - Excel will complain about this being an unsigned/untrusted add-in. Read and build the code yourself if you prefer.  

## Materialize demo set up

https://github.com/MaterializeInc/demos/tree/main/ecommerce-redpanda

## Known limitations

Only works with native Excel 365 on Windows.
- Excel for Mac does not support the native C API that Excel-DNA uses).
- Older versions of Excel don't support dynamic arrays.

Order by clauses are not implemented.

Large grids are currently inefficient due to copy from multiset to 2d array.

Only simple username login is implemented.