"use strict";

app.controller('AdminReportProjectController',
    function ($scope, AdminReportService, MessageService, CommmonService, $rootScope, toastr, listReport, DTOptionsBuilder, DTColumnDefBuilder) {
        $scope.listReport = listReport.data.Data;

        $scope.newReply = {
            ToUser: "",
            Title: "",
            Content: ""
        }


        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('order', [0, 'asc'])
        .withOption('stateSave', true)
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(6).notSortable()
        ];


        $scope.loadReportContent = function (reportid) {
            for (var i = 0; i < $scope.listReport.length; i++) {
                if ($scope.listReport[i].ReportID == reportid) {
                    $scope.reportContent = $scope.listReport[i];
                    $scope.newReply.ToUser = $scope.reportContent.ReporterUsername;
                    $scope.newReply.Title = "Trả lời về việc báo xấu dự án \"" + $scope.reportContent.ProjectTitle + "\"";
                    if ($scope.listReport[i].Status != "rejected" && $scope.listReport[i].Status != "done")
                        $scope.changeReportStatus($scope.reportContent.ReportID, "viewed");
                }
            }
        }
        $scope.sendReply = function () {
            if ($scope.newReply.Content.trim() !== "") {

                var promisePost = MessageService.sendMessage($scope.newReply);
                promisePost.then(
                    function (result) {
                        if (result.data.Status === "success") {
                            $('#sendQuestion').modal('hide');
                            $scope.newReply = {
                                Title: "",
                                Content: ""
                            }
                            toastr.success("Gửi câu hỏi thành công");
                        } else {
                            var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                            if (a) {
                                $scope.Error = result.data.Message;
                                toastr.error($scope.Error, 'Lỗi');
                            }
                        }
                    },
                    function (error) {
                        $scope.Error = error.data.Message;
                    });
            } else {
                toastr.warning("Bạn chưa nhập nội dung câu hỏi", 'Thông báo!');
            }
        }




        $scope.changeReportStatus = function (id, status) {
            var promise = AdminReportService.changeReportStatus(id, status, "Project");
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        for (var i = 0; i < $scope.listReport.length; i++) {
                            if ($scope.listReport[i].ReportID == id) {
                                if ($scope.listReport[i].Status != "done" && $scope.listReport[i].Status != "rejected")
                                    $scope.listReport[i].Status = status;
                                break;
                            }
                        }

                    } else {
                        var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        if (a) {
                            $scope.Error = result.data.Message;
                            toastr.error($scope.Error, 'Lỗi');
                        }
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }
    });