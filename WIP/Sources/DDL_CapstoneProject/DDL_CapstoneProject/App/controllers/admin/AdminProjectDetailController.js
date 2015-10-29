"use strict";

app.controller('AdminProjectDetailController',
    function ($scope, $rootScope, $sce, toastr, project, AdminProjectService,
    CommmonService, DTOptionsBuilder, DTColumnDefBuilder) {

        // Get project list
        $scope.Project = project.data.Data;

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        }

        // Function check string startwith 'http'
        $scope.checkHTTP = function (input) {
            var lowerStr = (input + "").toLowerCase();
            return lowerStr.indexOf('http') === 0;
        }

        $scope.FirstLoadComment = false;
        $scope.Project.CommentsList = {};
        $scope.FirstUpdateLogs = false;
        $scope.Project.UpdateLogsList = {};

        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('order', [3, 'asc'])
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(0).notSortable()
        ];

        // function get comments
        $scope.getCommentList = function () {
            if (!$scope.FirstLoadComment) {
                var promiseGet = AdminProjectService.getCommentList($scope.Project.ProjectCode, "");
                promiseGet.then(
                    function (result) {
                        if (result.data.Status === "success") {
                            $scope.Project.CommentsList = result.data.Data;
                            $scope.FirstLoadComment = true;
                        } else {
                            CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                            $scope.Error = result.data.Message;
                            toastr.error($scope.Error, 'Lỗi');
                        }
                    },
                    function (error) {
                        $scope.Error = error.data.Message;
                        toastr.error($scope.Error, 'Lỗi');
                    });
            }
        }
        // Function load more comment
        $scope.loadMoreComment = function () {
            var last = $scope.Project.CommentsList.length;
            var promisePut = AdminProjectService.getCommentList($scope.Project.ProjectCode, $scope.Project.CommentsList[last - 1].CreatedDate);
            promisePut.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.Project.CommentsList = $scope.Project.CommentsList.concat(result.data.Data);
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
        }

        // function get comments
        $scope.getUpdateLogList = function () {
            if (!$scope.FirstUpdateLogs) {
                var promiseGet = AdminProjectService.getUpdateLogList($scope.Project.ProjectCode);
                promiseGet.then(
                    function (result) {
                        if (result.data.Status === "success") {
                            $scope.Project.UpdateLogsList = result.data.Data;
                            $scope.Project.NumberUpdate = $scope.Project.UpdateLogsList.length;
                            $scope.FirstUpdateLogs = true;
                        } else {
                            CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                            $scope.Error = result.data.Message;
                            toastr.error($scope.Error, 'Lỗi');
                        }
                    },
                    function (error) {
                        $scope.Error = error.data.Message;
                        toastr.error($scope.Error, 'Lỗi');
                    });
            }
        }

        // function to get backing event
        $scope.loadlistBacker = function () {
            var promise = AdminProjectService.getListBacker($scope.Project.ProjectCode);
            promise.then(
                function (result) {
                    $scope.ListBacker = result.data.Data.listBacker;
                    $scope.labels = result.data.Data.Date;
                    $scope.series = ['Số tiền đã ủng hộ'];
                    $scope.cate = [];
                    for (var i = 0; i < 1000; i++) {
                        $scope.cate.push(i);
                    }


                    $scope.highchartsNG = {
                        options: {
                            chart: {
                                type: 'line'
                            }
                        },
                        xAxis: {
                            categories: $scope.cate
                        },
                        yAxis: [{ // Primary yAxis
                            labels: {
                                format: '{value} VNĐ',
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
                        }],
                        series: [{
                            name: 'Số tiền ủng hộ',
                            data: result.data.Data.Amount
                        }],
                        title: {
                            text: 'Biểu đồ thống kê số tiền được ủng hộ'
                        },
                        loading: false,
                        credits: {
                            enabled: false
                        },
                    }

                    $scope.highchartsNG.xAxis.categories = result.data.Data.Date;
                }
            );
        };

        $scope.loadlistBacker();

        $scope.change = function (status) {
            $scope.Project.Status = status;
            var promisePost = AdminProjectService.changeProjectStatus($scope.Project);

            promisePost.then(
                function (result) {
                    if (result.data.Status === "success") {
                        toastr.success('Chỉnh sửa thành công!');
                        //$location.path("/project/edit/" + result.data.Data).replace();
                    } else {
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        }

    });