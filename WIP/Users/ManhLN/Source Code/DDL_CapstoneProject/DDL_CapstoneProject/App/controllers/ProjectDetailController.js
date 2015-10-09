"use strict";

app.controller('ProjectDetailController', function ($scope, $sce, $rootScope, toastr, project, ProjectService, CommmonService) {
    //Todo here.
    $scope.Project = project.data.Data;

    $scope.CurrentUser = $rootScope.UserInfo;
    $scope.NewComment = {};
    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    // Function to submit comment
    $scope.submitComment = function () {
        if ($scope.NewComment.CommentContent.trim() !== "") {
            $scope.NewComment.UserName = $scope.CurrentUser.UserName;
            var promisePut = ProjectService.Comment($scope.Project.ProjectCode, $scope.NewComment);
            promisePut.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.Project.CommentsList.unshift(result.data.Data);
                        $scope.NewComment = "";
                        toastr.success('Bình luận thành công!');
                    } else {
                        CommmonService.checkError(result.data.Type);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        } else {
            toastr.warning("Bạn chưa nhập nội dung bình luận", 'Thông báo!');
        }
    }

    // Function hide/unhide comment
    $scope.showHideComment = function (index) {
        var promisePut = ProjectService.ShowHideComment($scope.Project.CommentsList[index].CommentID);
        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Project.CommentsList[index].IsHide = result.data.Data.IsHide;
                    toastr.success('Thành công!');
                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                $scope.Error = result.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    }

    // Function hide/unhide comment
    $scope.loadMoreComment = function () {
        var skip = $scope.Project.CommentsList.length;
        var promisePut = ProjectService.loadMoreComment($scope.Project.CommentsList[index].CommentID,skip);
        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Project.CommentsList[index].IsHide = result.data.Data.IsHide;
                    toastr.success('Thành công!');
                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                $scope.Error = result.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    }

    // Function check string startwith 'http'
    $scope.checkHTTP = function (input) {
        var lowerStr = (input + "").toLowerCase();
        return lowerStr.indexOf('http') === 0;
    }
});