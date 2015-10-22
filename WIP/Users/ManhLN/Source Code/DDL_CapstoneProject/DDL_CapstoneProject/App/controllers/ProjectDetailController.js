"use strict";

app.controller('ProjectDetailController', function ($scope, $sce, $rootScope, toastr, project, ProjectService, CommmonService, DTOptionsBuilder, DTColumnDefBuilder) {
    //Todo here.
    $scope.Project = project.data.Data;
    $scope.FirstUpdateLogs = false;
    $scope.Project.UpdateLogsList = {};
    $scope.FirstLoadComment = false;
    $scope.Project.CommentsList = {}
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

    // function get comments
    $scope.getCommentList = function () {
        if (!$scope.FirstLoadComment) {
            var promiseGet = ProjectService.getCommentList($scope.Project.ProjectCode, "");
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

    // function get comments
    $scope.getUpdateLogList = function () {
        if (!$scope.FirstUpdateLogs) {
            var promiseGet = ProjectService.getUpdateLogList($scope.Project.ProjectCode);
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
        //    toastr.warning("Tối thiểu 10, tối đa 200 kí tự", 'Thông báo');
        //} else {
        $scope.NewComment.UserName = $scope.CurrentUser.UserName;
        var promisePut = ProjectService.Comment($scope.Project.ProjectCode, $scope.NewComment, $scope.Project.CommentsList[0].CreatedDate);
        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Project.CommentsList = result.data.Data.concat($scope.Project.CommentsList);
                    $scope.Project.NumberComment += result.data.Data.length;
                    $scope.NewComment = {};
                    toastr.success('Bình luận thành công');
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
        //}
    }

    // Function hide/unhide comment
    $scope.showHideComment = function (index) {
        var promisePut = ProjectService.ShowHideComment($scope.Project.CommentsList[index].CommentID);
        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Project.CommentsList[index] = result.data.Data;
                    toastr.success('Thành công');
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

    // Function load more comment
    $scope.loadMoreComment = function () {
        var last = $scope.Project.CommentsList.length;
        var promisePut = ProjectService.getCommentList($scope.Project.ProjectCode, $scope.Project.CommentsList[last - 1].CreatedDate);
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

    // Function edit comment.
    $scope.editComment = function (index) {
        if ($scope.Project.CommentsList[index].EditedCommentContent !== $scope.Project.CommentsList[index].CommentContent
            && $scope.Project.CommentsList[index].EditedCommentContent.length >= 10
            && $scope.Project.CommentsList[index].EditedCommentContent.length <= 500) {
            var promisePut = ProjectService.editComment($scope.Project.CommentsList[index].CommentID, $scope.Project.CommentsList[index].EditedCommentContent);
            promisePut.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.Project.CommentsList[index] = result.data.Data;
                        $scope.showEditForm(index);
                        toastr.success('Sửa bình luận thành công');
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
        } else if ($scope.Project.CommentsList[index].EditedCommentContent.length < 10
                    || $scope.Project.CommentsList[index].EditedCommentContent.length > 500) {
            // Do nothing.
        }
        else {
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
                    $scope.Project.NumberComment--;
                    toastr.success('Xóa bình luận thành công');
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

    $scope.remind = function () {
        var promise = ProjectService.remindProject($scope.Project.ProjectCode);
        promise.then(
          function (result) {
              if (result.data.Status === "success") {
                  toastr.success('Theo dõi dự án thành công');
              } else if (result.data.Status === "error") {
                  $scope.Error = result.data.Message;
                  toastr.error($scope.Error, 'Lỗi');
              }
          }
       );
    };

    $scope.Reminded = true;
    $scope.report = function () {
        var promise = ProjectService.reportProject($scope.Project.ProjectCode, $scope.ReportContent);
        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Báo cáo sai phạm thành công');
                } else if (result.data.Status === "error") {
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Bạn chưa đăng nhập');
                }
            }
         );
    };

    $scope.loadlistBacker = function () {
        var promise = ProjectService.getListBacker($scope.Project.ProjectCode);
        promise.then(
            function (result) {
                $scope.ListBacker = result.data.Data;
            }
         );
    };

    // Define table
    $scope.dtOptions = DTOptionsBuilder.newOptions()
    .withDisplayLength(10)
    .withOption('order', [3, 'desc'])
    .withBootstrap();

    $scope.dtColumnDefs = [
        DTColumnDefBuilder.newColumnDef(0).notSortable()
    ];


});