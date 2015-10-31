﻿"use strict";

app.controller('ProjectDetailController', function ($scope, $sce, $rootScope, toastr, project, ProjectService, CommmonService, DTOptionsBuilder, DTColumnDefBuilder, $filter, MessageService) {
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
        var lastdatetime = $scope.Project.CommentsList.length > 0 ? $scope.Project.CommentsList[0].CreatedDate : "";
        var promisePut = ProjectService.Comment($scope.Project.ProjectCode, $scope.NewComment, lastdatetime);
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
              if (result.data.Status == "success" && result.data.Message == "reminded") {
                  $scope.Project.Reminded = true;
                  toastr.success('Theo dõi dự án thành công');

              }
              else if (result.data.Status == "success" && result.data.Message == "notremind") {
                  $scope.Project.Reminded = false;
                  toastr.success('Hủy theo dõi dự án thành công');
              }
              else if (result.data.Status == "error") {
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

    $scope.checkLoadlist = false;
    $scope.loadlistBacker = function () {
        if($scope.checkLoadlist == false){
        var promise = ProjectService.getListBacker($scope.Project.ProjectCode);
        promise.then(
            function (result) {
                $scope.ListBacker = result.data.Data.listBacker;
                $scope.labels = result.data.Data.Date;
                $scope.series = ['Số tiền đã ủng hộ','Mục tiêu'];
                var data2 = [];
                for (var i = 0; i < result.data.Data.Amount.length; i++) {
                    data2.push($scope.Project.FundingGoal);
                }
                $scope.data = [result.data.Data.Amount, data2];
                $scope.colours = ['#97bbcd', '#f7464a'];
                //$scope.checkLoadlist = true;
                //$scope.labels = ["January", "February", "March", "April", "May", "June", "July"];
                //$scope.series = ['Series A', 'Series B'];
                //$scope.data = [
                //  [65, 59, 80, 81, 56, 55, 40],
                //  [28, 48, 40, 19, 86, 27, 90]
                //];
            }
         );
        }
    };

    // Define table
    $scope.dtOptions = DTOptionsBuilder.newOptions()
    .withDisplayLength(10)
    .withOption('order', [3, 'desc'])
    .withBootstrap();

    $scope.dtColumnDefs = [
        DTColumnDefBuilder.newColumnDef(0).notSortable()
    ];

    $scope.NewQuestion = {
        Title: "",
        Content: ""
    }

    // Function request 
    $scope.sendQuestion = function () {
        if ($scope.NewQuestion.Content.trim() !== "") {
            $scope.NewQuestion.ToUser = $scope.Project.Creator.UserName;
            $scope.NewQuestion.Title = "Gửi câu hỏi về dự án \"" + $scope.Project.Title + "\"";
            var promisePost = MessageService.sendMessage($scope.NewQuestion);
            promisePost.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $('#sendQuestion').modal('hide');
                        $scope.NewQuestion = {
                            Title: "",
                            Content: ""
                        }
                        toastr.success("Gửi câu hỏi thành công");
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        } else {
            toastr.warning("Bạn chưa nhập nội dung câu hỏi", 'Thông báo!');
        }
    }

});