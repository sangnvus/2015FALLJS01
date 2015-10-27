"use strict";

app.controller('AdminProjectDetailController',
    function ($scope, $rootScope, $sce, toastr, project, AdminProjectService,
    CommmonService) {

        // Get project list
        $scope.Project = project.data.Data;

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        }

        $scope.FirstLoadComment = false;
        $scope.Project.CommentsList = {};
        $scope.FirstUpdateLogs = false;
        $scope.Project.UpdateLogsList = {};

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

        $scope.save = function () {
            //$scope.Project.CategoryId = $scope.selectedOption.CategoryID;

            //var promisePost = ProjectService.createProject($scope.Project);

            //promisePost.then(
            //    function (result) {
            //        if (result.data.Status === "success") {
            //            toastr.success('Bạn đã khởi tạo dự án thành công!', 'Thành công!');
            //            $location.path("/project/edit/" + result.data.Data).replace();
            //        } else {
            //            CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
            //            $scope.Error = result.data.Message;
            //            toastr.error($scope.Error, 'Lỗi!');
            //        }
            //    },
            //    function (error) {
            //        $scope.Error = error.data.Message;
            //        toastr.error($scope.Error, 'Lỗi!');
            //    });
        }

    });