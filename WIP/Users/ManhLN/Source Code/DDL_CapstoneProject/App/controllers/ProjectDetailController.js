"use strict";

app.controller('ProjectDetailController', function ($scope, $sce, $rootScope, toastr, project, ProjectService, CommmonService) {
    //Todo here.
    $scope.Project = project.data.Data;

    $scope.CurrentUser = $rootScope.UserInfo;
    $scope.NewComment = {};

    $scope.IsEdit = [];

    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    // Function check string startwith 'http'
    $scope.checkHTTP = function (input) {
        var lowerStr = (input + "").toLowerCase();
        return lowerStr.indexOf('http') === 0;
    }

    // Function show edit comment form.
    $scope.showEditForm = function (index) {
        if ($scope.IsEdit[index] == null || $scope.IsEdit[index] === false) {
            $scope.IsEdit[index] = true;
            $scope.Project.CommentsList[index].EditedCommentContent = $scope.Project.CommentsList[index].CommentContent;
        } else {
            $scope.IsEdit[index] = false;
        }
    }

    // Function to submit comment
    $scope.submitComment = function () {
        //if (form.$dirty === false || (form.$dirty && form.$invalid)) {
        //    toastr.warning("Tối thiểu 10, tối đa 200 kí tự", 'Thông báo!');
        //} else {
        $scope.NewComment.UserName = $scope.CurrentUser.UserName;
        var promisePut = ProjectService.Comment($scope.Project.ProjectCode, $scope.NewComment, $scope.Project.CommentsList[0].CreatedDate);
        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Project.CommentsList = result.data.Data.concat($scope.Project.CommentsList);
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
                toastr.error($scope.Error, 'Lỗi!');
            });
        //}
    }

    // Function hide/unhide comment
    $scope.showHideComment = function (index) {
        var promisePut = ProjectService.ShowHideComment($scope.Project.CommentsList[index].CommentID);
        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Project.CommentsList[index] = result.data.Data;
                    toastr.success('Thành công!');
                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    }

    // Function load more comment
    $scope.loadMoreComment = function () {
        var skip = $scope.Project.CommentsList.length;
        var promisePut = ProjectService.loadMoreComment($scope.Project.CommentsList[index].CommentID, skip);
        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Project.CommentsList[index] = result.data.Data;
                    toastr.success('Thành công!');
                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    }

    // Function edit comment.
    $scope.editComment = function (index) {
        if ($scope.Project.CommentsList[index].EditedCommentContent !== $scope.Project.CommentsList[index].CommentContent) {
            var promisePut = ProjectService.editComment($scope.Project.CommentsList[index].CommentID, $scope.Project.CommentsList[index].EditedCommentContent);
            promisePut.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.Project.CommentsList[index] = result.data.Data;
                        $scope.showEditForm(index);
                        toastr.success('Sửa bình luận', 'Thành công!');
                    } else {
                        CommmonService.checkError(result.data.Type);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        } else {
            $scope.showEditForm(index);
        }
    }

    // Function edit comment.
    $scope.deleteComment = function (index) {
        var promise = ProjectService.deleteComment($scope.Project.CommentsList[index].CommentID);
        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Project.CommentsList.splice(index, 1);
                    toastr.success('Xóa bình luận', 'Thành công!');
                } else {
                    CommmonService.checkError(result.data.Type);
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