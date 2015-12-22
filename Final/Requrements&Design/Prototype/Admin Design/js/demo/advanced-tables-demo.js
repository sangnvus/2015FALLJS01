//DataTables Initialization
$(document).ready(function() {
    $('#example-table').dataTable({"aoColumnDefs": [
		{ "bSortable": false, "aTargets": [ 0 ] }
	]});
	
	$('#example-table-2').dataTable({"aoColumnDefs": [
		{ "bSortable": false, "aTargets": [ 0 ] }
	]});
});
