// Flex Admin Demo Chart JavaScript


// Disables line smoothing
//Morris Line Chart
// ID of the element in which to draw the chart.
Morris.Line({
element: 'morris-chart-line',
// Chart data records -- each entry in this array corresponds to a point on
// the chart.
data: [
  { d: '2015-01-01', projects: 9, success: 5, fail: 4 },
  { d: '2015-02-01', projects: 9, success: 5, fail: 4 },
  { d: '2015-03-01', projects: 9, success: 5, fail: 4 },
  { d: '2015-04-01', projects: 9, success: 5, fail: 4 },
  { d: '2015-05-01', projects: 9, success: 5, fail: 4 },
  { d: '2015-06-01', projects: 9, success: 5, fail: 4 },
  { d: '2015-07-01', projects: 9, success: 5, fail: 4 },
  { d: '2015-08-01', projects: 9, success: 5, fail: 4 },
  { d: '2015-09-01', projects: 9, success: 5, fail: 4 },
  { d: '2015-09-26', projects: 9, success: 5, fail: 4 },
],
// The name of the data record attribute that contains x-visitss.
xkey: 'd',
// A list of names of data record attributes that contain y-visitss.
ykeys: ['projects', 'success', 'fail'],
// Labels for the ykeys -- will be displayed when you hover over the
// chart.
lineColors: ['#16a085','#f39c12', '#350035'],
labels: ['projects', 'success', 'fail'],
smooth: false,
resize: true
});