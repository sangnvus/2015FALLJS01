"use strict";

app.controller('AdminProjectDetailController',
    function ($scope, $rootScope, $sce, toastr, project, AdminProjectService,
    CommmonService, DTOptionsBuilder, DTColumnDefBuilder, SweetAlert) {

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
                    $scope.series = ['Số tiền đã được ủng hộ', 'Mục tiêu'];
                    var data2 = [];
                    for (var i = 0; i < result.data.Data.Amount.length; i++) {
                        data2.push($scope.Project.FundingGoal);
                    }
                    $scope.data = [result.data.Data.Amount, data2];
                    $scope.colours = ['#97bbcd', '#f7464a'];
                    $scope.checkLoadlist = true;
                    $scope.options = {
                        multiTooltipTemplate: function (label) {
                            return (label.datasetLabel + ': ' + label.value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")) + "₫";
                        },
                        scaleLabel: function (label) {
                            return (label.value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")) + "₫";
                        }
                    };
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
        };

        // Alert admin before change status
        $scope.warning = function (status) {
            var stringAlert = '';
            if (status === 'suspended') {
                stringAlert = 'ngừng';
            }

            if (status === 'approved') {
                stringAlert = 'thông qua';
            }

            if (status === 'rejected') {
                stringAlert = 'từ chối';
            }
            SweetAlert.swal({
                title: "Bạn có chắc chắn muốn " + stringAlert + " dự án này không?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "Không",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //SweetAlert.swal("Edited!", "Project's basic has been edited.", "success");
                        $scope.change(status);
                    } else {
                        //SweetAlert.swal("Cancelled", "Project's basic is safe :)", "error");
                    }
                });
        };

    });