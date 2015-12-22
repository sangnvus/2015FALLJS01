//Morris Line Chart
// ID of the element in which to draw the chart.
Morris.Line({
element: 'morris-chart-line',
// Chart data records -- each entry in this array corresponds to a point on
// the chart.
data: [
  { d: '2015-10-01', Back: 802, Fund: 1651 },
  { d: '2015-10-02', Back: 783, Fund: 1598 },
  { d: '2015-10-03', Back:  820, Fund: 1684 },
  { d: '2015-10-04', Back: 839, Fund: 1488 },
  { d: '2015-10-05', Back: 792, Fund: 1465 },
  { d: '2015-10-06', Back: 859, Fund: 1521 },
  { d: '2015-10-07', Back: 790, Fund: 1598 },
  { d: '2015-10-08', Back: 1680, Fund: 2615 },
  { d: '2015-10-09', Back: 1592, Fund: 3165 },
  { d: '2015-10-10', Back: 1420, Fund: 1798 },
  { d: '2015-10-11', Back: 882, Fund: 1754 },
  { d: '2015-10-12', Back: 889, Fund: 1654 },
  { d: '2015-10-13', Back: 819, Fund: 1521 },
  { d: '2015-10-14', Back: 849, Fund: 1794 },
  { d: '2015-10-15', Back: 870, Fund: 1549 },
  { d: '2015-10-16', Back: 1063, Fund: 2615 },
  { d: '2015-10-17', Back: 1192, Fund: 2498 },
  { d: '2015-10-18', Back: 1224, Fund: 2684 },
  { d: '2015-10-19', Back: 1329, Fund: 2888 },
  { d: '2015-10-20', Back: 1329, Fund: 2998 },
  { d: '2015-10-21', Back: 1239, Fund: 2945 },
  { d: '2015-10-22', Back: 1190, Fund: 2151 },
  { d: '2015-10-23', Back: 1312, Fund: 2632 },
  { d: '2015-10-24', Back: 1293, Fund: 2361 },
  { d: '2015-10-25', Back: 1283, Fund: 2451 },
  { d: '2015-10-26', Back: 1248, Fund: 2416 },
  { d: '2015-10-27', Back: 1323, Fund: 2684 },
  { d: '2015-10-28', Back: 1390, Fund: 2165 },
  { d: '2015-10-29', Back: 1420, Fund: 2864 },
  { d: '2015-10-30', Back: 1529, Fund: 3021 },
  { d: '2015-10-31', Back: 1892, Fund: 4251 },
],
// The name of the data record attribute that contains x-visitss.
xkey: 'd',
// A list of names of data record attributes that contain y-visitss.
ykeys: ['Back', 'Fund'],
// Labels for the ykeys -- will be displayed when you hover over the
// chart.
lineColors: ['#16a085','#f39c12'],
labels: ['Back', 'Fund'],
smooth: false,
resize: true
});