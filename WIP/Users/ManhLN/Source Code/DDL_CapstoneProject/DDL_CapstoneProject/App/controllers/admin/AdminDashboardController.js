"use strict";

app.controller('AdminDashBoardController', function ($scope, $rootScope, $sce, toastr, basicInfo, listcategory, topBackers, statistic, AdminUserService, AdminProjectService,
    CommmonService, DTOptionsBuilder, DTColumnDefBuilder) {
    // Define table
    $scope.dtOptions = DTOptionsBuilder.newOptions()
    .withDisplayLength(10)
    .withOption('order', [0, 'desc'])
    .withBootstrap();

    // initail statistic chart year option
    $scope.chartYear = [];
    var d = new Date();
    var n = d.getFullYear();
    var currentYear = parseInt(n);
    for (var l = 2015; l <= currentYear; l++) {
        $scope.chartYear.push({
            name: l,
            value: l
        });
    };
    $scope.selectedChartYear = $scope.chartYear[$scope.chartYear.length - 1];

    $scope.tableOption = [
        {
            name: 'Năm',
            value: 'year'
        }, {
            name: 'Tháng',
            value: 'month'
        }
    ];
    $scope.selectedTableOption = $scope.tableOption[0];

    //$scope.dtColumnDefs = [
    //    DTColumnDefBuilder.newColumnDef(0).notSortable()
    //];

    // Get basic info
    $scope.BasicInfo = basicInfo.data.Data;
    // Get category list
    $scope.Categories = {
        Labels: [],
        Data: []
    };
    // Get top backers
    $scope.TopBacker = topBackers.data.Data;
    // Get project statistic
    $scope.Statistic = statistic.data.Data;
    // Set data for month
    $scope.OrgiData = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

    $scope.cate = [];
    for (var i = 1; i <= 12; i++) {
        $scope.cate.push(i);
    }

    $scope.setStatisticChart = function () {
        var j = 0;
        var k = 0;
        $scope.Created = angular.copy($scope.OrgiData);
        for (j = 0; j < 13; j++) {
            for (k = 0; k < $scope.Statistic.Created.length; k++) {
                if (j === $scope.Statistic.Created[k].Month) {
                    $scope.Created[j - 1] = $scope.Statistic.Created[k].Amount;
                }
            }
        };

        $scope.Succeed = angular.copy($scope.OrgiData);
        for (j = 0; j < 13; j++) {
            for (k = 0; k < $scope.Statistic.Succeed.length; k++) {
                if (j === $scope.Statistic.Succeed[k].Month) {
                    $scope.Succeed[j - 1] = $scope.Statistic.Succeed[k].Amount;
                }
            }
        };

        $scope.Fail = angular.copy($scope.OrgiData);
        for (j = 0; j < 13; j++) {
            for (k = 0; k < $scope.Statistic.Fail.length; k++) {
                if (j === $scope.Statistic.Fail[k].Month) {
                    $scope.Fail[j - 1] = $scope.Statistic.Fail[k].Amount;
                }
            }
        };

        $scope.Funed = angular.copy($scope.OrgiData);
        for (j = 0; j < 13; j++) {
            for (k = 0; k < $scope.Statistic.Funded.length; k++) {
                if (j === $scope.Statistic.Funded[k].Month) {
                    $scope.Funed[j - 1] = $scope.Statistic.Funded[k].Amount;
                }
            }
        };

        $scope.Profit = angular.copy($scope.OrgiData);
        for (j = 0; j < 13; j++) {
            for (k = 0; k < $scope.Statistic.Profit.length; k++) {
                if (j === $scope.Statistic.Profit[k].Month) {
                    $scope.Profit[j - 1] = $scope.Statistic.Profit[k].Amount;
                }
            }
        };

        $scope.highchartsNG = {
            options: {
                chart: {
                    type: 'xy'
                }
            },
            xAxis: {
                categories: $scope.cate
            },
            yAxis: [
                {
                    // Primary yAxis
                    labels: {
                        pointFormat: "Value: {point.y:,.0f} VNĐ",
                        //format: '{value} VNĐ',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    title: {
                        text: 'Số tiền được ủng hộ',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    }
                }, { // Secondary yAxis
                    title: {
                        text: 'Số dự án',
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
                }
            ],
            series: [{
                type: 'column',
                name: 'Tạo',
                yAxis: 1,
                data: $scope.Created
            }, {
                type: 'column',
                name: 'Thành công',
                yAxis: 1,
                data: $scope.Succeed
            }, {
                type: 'column',
                name: 'Thất bại',
                yAxis: 1,
                data: $scope.Fail
            }, {
                type: 'spline',
                name: 'Ủng hộ',
                data: $scope.Funed,
                tooltip: {
                    valueSuffix: ' VNĐ'
                }
            }, {
                type: 'spline',
                name: 'Lợi nhuận',
                data: $scope.Profit,
                tooltip: {
                    valueSuffix: ' VNĐ'
                }
            }],
            title: {
                text: 'Thống kê thông số dự án'
            },
            loading: false,
            credits: {
                enabled: false
            },
        };
    };

    $scope.setStatisticChart();

    // Pass data to donut_Cate chart
    for (var i = 0; i < listcategory.data.Data.length; i++) {
        $scope.Categories.Labels.push(listcategory.data.Data[i].Name);
        $scope.Categories.Data.push(listcategory.data.Data[i].ProjectCount);
    };

    // Donut chart for rank project
    $scope.donut_Rank = {
        Labels: ["Rank A", "Rank B", "Rank C", "Rank D"],
        Data: [$scope.BasicInfo.RankA, $scope.BasicInfo.RankB, $scope.BasicInfo.RankC, $scope.BasicInfo.RankD]
    };

    // Donut chart for success rate project
    $scope.donut_Rate = {
        Labels: ["Thành công", "Thất bại"],
        Data: [$scope.BasicInfo.TotalSucceed, $scope.BasicInfo.TotalFail]
    };

    // Get recent user
    $scope.getRecentUser = function () {
        var promise = AdminUserService.getRecentUser();

        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.RecentUser = result.data.Data;
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
    $scope.getRecentUser();

    // Get top project
    $scope.getTopProject = function () {
        var promise = AdminProjectService.getTopProject();

        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.TopProject = result.data.Data;
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
    $scope.getTopProject();

    // Get recent backing
    $scope.getRecentBacking = function () {
        var promise = AdminUserService.getRecentBacking();

        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.RecentBacking = result.data.Data;
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
    $scope.getRecentBacking();

    // Get recent backing
    $scope.changeTable = function () {
        var promise = AdminProjectService.getProjectStatisticTable($scope.selectedTableOption.value);

        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.StatisticTable = result.data.Data;
                    if ($scope.StatisticTable[0].Time.length > 4) {
                        for (var j = 0; j < $scope.StatisticTable.length; j++) {
                            var from = $scope.StatisticTable[j].Time.split("/");
                            $scope.StatisticTable[j].Time = new Date(from[1], from[0] - 1);
                        }
                    } else {
                        for (var j = 0; j < $scope.StatisticTable.length; j++) {
                            $scope.StatisticTable[j].Time = new Date($scope.StatisticTable[j].Time);
                        }
                    }
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
    $scope.changeTable($scope.selectedTableOption.value);

    // Update statistic chart
    $scope.updateChart = function () {
        var promise = AdminProjectService.getProjectStatistic($scope.selectedChartYear.value);

        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Statistic = result.data.Data;
                    $scope.setStatisticChart();
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };

});