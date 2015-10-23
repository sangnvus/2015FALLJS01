"use strict";

app.controller('PublicProfileController',
    function ($scope, $sce, $rootScope, $window, $location, userpublicinfo, $filter,
        toastr, SweetAlert, MessageService, CommmonService) {
        $scope.UserBasicInfo = userpublicinfo.data.Data;

        //$scope.UserBasicInfo.CreatedDate = new Date($filter('date')($scope.UserBasicInfo.CreatedDate, "yyyy-MM-dd"));
        //$scope.UserBasicInfo.LastLogin = new Date($filter('date')($scope.UserBasicInfo.LastLogin, "yyyy-MM-dd"));
        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        }

        // Function check string startwith 'http'
        $scope.checkHTTP = function (input) {
            var lowerStr = (input + "").toLowerCase();
            return lowerStr.indexOf('http') === 0;
        }


        $scope.showContactBox = function () {
            if ($rootScope.UserInfo.IsAuthen === false) {
                SweetAlert.swal({
                    title: "Chưa đăng nhập",
                    text: "Bạn có muốn đăng nhập để gửi tin nhắn cho người này?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#2ecc71",
                    confirmButtonText: "Có, đăng nhập ngay",
                    cancelButtonText: "Hủy",
                    closeOnConfirm: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            $window.location.href = $rootScope.BaseUrl + "login?returnUrl=" + $location.url();
                        }
                    });
            } else {
                $("#sentBox").modal("show");
            }
        }

        // Function request 
        $scope.sendMessage = function () {
            if ($scope.NewMessage.Content.trim() !== "") {
                var promisePost = MessageService.sendMessage($scope.NewMessage);
                promisePost.then(
                    function (result) {
                        if (result.data.Status === "success") {
                            $('#sentBox').modal('hide');
                            $scope.NewMessage = {
                                Title: "",
                                Content: ""
                            }
                            toastr.success("Gửi tin nhắn thành công");
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
                toastr.warning("Bạn chưa nhập nội dung tin nhắn", 'Thông báo!');
            }
        }
    });