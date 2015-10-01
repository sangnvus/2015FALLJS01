$(function () {
    $('#container').highcharts({
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: 'Monthly Statistic'
        },
        xAxis: {
            categories: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10']
        },
        yAxis: [{ // Primary yAxis
            labels: {
                format: '{value} $',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            },
            title: {
                text: 'Money',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            }
        }, { // Secondary yAxis
            title: {
                text: 'Project',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            labels: {
                format: '{value}',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            opposite: true
        }],
        tooltip: {
            shared: true
        },
        series: [{
            type: 'column',
            name: 'Created',
            yAxis: 1,
            data: [3, 2, 1, 3, 4, 3, 2, 1, 3, 4]
        }, {
            type: 'column',
            name: 'Success',
            yAxis: 1,
            data: [2, 3, 5, 7, 6, 2, 3, 5, 7, 6]
        }, {
            type: 'column',
            name: 'Fail',
            yAxis: 1,
            data: [4, 3, 3, 9, 3, 4, 3, 3, 9, 3]
        }, {
            type: 'spline',
            name: 'Fund',
            data: [3000, 2670, 3000, 6330, 3330, 3000, 2670, 3000, 6330, 3330],
            tooltip: {
                valueSuffix: ' $'
            },
        },  {
            type: 'spline',
            name: 'Profit',
            data: [1000, 500, 150, 633, 700, 1000, 500, 150, 633, 700],
            tooltip: {
                valueSuffix: ' $'
            },
        }]
    });
});

//----------------Morris Donut Chart -------------------- //
// category
Morris.Donut({
		element: 'category-donut',
		data: [
		 {label: "Art", value: 42.7},
		 {label: "Comic", value: 8.3},
		 {label: "Game", value: 12.8},
		 {label: "Film", value: 36.2}
		],
		resize: true,
		colors: ['#16a085','#2980b9', '#f39c12', '#e74c3c'],
		formatter: function (y) { return y + "%" ;}
	}
);
// rank
Morris.Donut({
		element: 'rank-donut',
		data: [
		 {label: "A", value: 42.7},
		 {label: "B", value: 8.3},
		 {label: "C", value: 12.8},
		 {label: "D", value: 36.2}
		],
		resize: true,
		colors: ['#16a085','#2980b9', '#f39c12', '#e74c3c'],
		formatter: function (y) { return y + "%" ;}
	}
);
// rate
Morris.Donut({
		element: 'rate-donut',
		data: [
		 {label: "Success", value: 42},
		 {label: "Fail", value: 58},
		],
		resize: true,
		colors: ['#16a085','#2980b9'],
		formatter: function (y) { return y + "%" ;}
	}
);
	