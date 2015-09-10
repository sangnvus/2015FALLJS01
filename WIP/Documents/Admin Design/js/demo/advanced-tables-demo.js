//DataTables Initialization
$(document).ready(function() {
    $('#example-table').dataTable({"aoColumnDefs": [
		{ "bSortable": false, "aTargets": [ 0 ] }
]});
});
