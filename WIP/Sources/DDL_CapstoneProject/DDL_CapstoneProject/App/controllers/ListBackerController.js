"use strict"

app.controller('ListBackerController', function ($scope, $route, projects, UserService, ProjectService, DTOptionsBuilder, DTColumnDefBuilder,CommmonService) {
    
    var promise = ProjectService.getProjectDetail($route.current.params.code);
    promise.then(

        function (result) {
            $scope.ListBacker = projects.data.Data.listBacker;
            $scope.labels = projects.data.Data.Date;
            $scope.series = ['Số tiền đã được ủng hộ', 'Mục tiêu'];
            var data2 = [];
            for (var i = 0; i < projects.data.Data.Amount.length; i++) {
                data2.push(result.data.Data.FundingGoal);
            }
            $scope.data = [projects.data.Data.Amount, data2];
            $scope.colors = ['#97bbcd', '#f7464a'];
            $scope.options = {
                multiTooltipTemplate: function (label) {
                    return (label.datasetLabel + ': ' + label.value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")) + "₫";
                },
                scaleLabel: function (label) {
                    return (label.value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")) + "₫";
                }
            };
        },
         function (error) {
             $scope.Error = error.data.Message;
         });

    $scope.exportExcel = function () {

        var list = [];
        var promise = UserService.GetBackingForCreatedProjectExport($route.current.params.code);
        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    list = $scope.Backer = result.data.Data;
                    list.unshift(['Mã dự án',
                        'Tên dự án',
                        'Mã gói',
                        'Mô tả gói',
                        'Ngày chuyển giao',
                        'Mã ủng hộ',
                        'Số tiền ủng hộ',
                        'Số lượng',
                        'Mô tả ủng hộ',
                        'Ngày ủng hộ',
                        'Người ủng hộ',
                        'Tên đăng nhập',
                        'Email',
                        'Địa chỉ',
                        'SĐT']);
                    CommmonService.exportExcel(list, "Danh sách Ủng Hộ - " + $route.current.params.code);
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });

    }
    $scope.dtOptions = DTOptionsBuilder.newOptions()
    .withDisplayLength(10)
    .withOption('order', [3, 'desc'])
    .withBootstrap();

    $scope.dtColumnDefs = [
        DTColumnDefBuilder.newColumnDef(4).notSortable()
    ];

    $scope.getBackingInfo = function () {
        var promiseGet = ProjectService.backingInfo($route.current.params.code);
        promiseGet.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.BackingInfo = result.data.Data[0];
                    //console.log($scope.BackingInfo[0].BackingDiscription)
                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }

       
});
