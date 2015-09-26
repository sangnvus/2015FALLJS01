// Flex Admin Demo Chart JavaScript


// Disables line smoothing
//Morris Line Chart
// ID of the element in which to draw the chart.
Morris.Line({
element: 'morris-chart-line',
// Chart data records -- each entry in this array corresponds to a point on
// the chart.
data: [
  { d: '2015-01-01', fund: 10000, profit: 1000 },
  { d: '2015-02-01', fund: 8000, profit: 3000 },
  { d: '2015-03-01', fund: 40000, profit: 10000 },
  { d: '2015-04-01', fund: 2000, profit: 500 },
  { d: '2015-05-01', fund: 50000, profit: 12000 },
  { d: '2015-06-01', fund: 10000, profit: 1000 },
  { d: '2015-07-01', fund: 10000, profit: 1000 },
  { d: '2015-08-01', fund: 10000, profit: 1000 },
  { d: '2015-09-01', fund: 10000, profit: 1000 },
  { d: '2015-09-26', fund: 10000, profit: 1000 },
],
// The name of the data record attribute that contains x-visitss.
xkey: 'd',
// A list of names of data record attributes that contain y-visitss.
ykeys: ['fund', 'profit'],
// Labels for the ykeys -- will be displayed when you hover over the
// chart.
lineColors: ['#16a085','#f39c12'],
labels: ['fund', 'profit'],
smooth: false,
resize: true
});