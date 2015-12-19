"use strict";

app.controller('PaymentProjectController', function ($scope, $rootScope, $sce, $route, $window, $location, toastr, CommmonService, ProjectService, rewardPkgs, usereditinfo, project) {
    // Get rewardPkgs
    $scope.RewardPkgs = rewardPkgs.data.Data;
    // Get Project basic information
    $scope.Project = project.data.Data;
    // Get backingdata form backing page
    $scope.BackData = ProjectService.getBack();
    // Get user basic information
    $scope.UserBasicInfo = usereditinfo.data.Data;

    // Check submit
    $scope.submitAble = false;
    if ($scope.UserBasicInfo.FullName != null && $scope.UserBasicInfo.FullName != ''
        && $scope.UserBasicInfo.Email != null && $scope.UserBasicInfo.Email != ''
        && $scope.UserBasicInfo.Addres != null && $scope.UserBasicInfo.Addres != ''
        && $scope.UserBasicInfo.ContactNumber != null && $scope.UserBasicInfo.ContactNumber != '') {
        $scope.submitAble = true;
    }

    if ($scope.UserBasicInfo.ProfileImage.indexOf("/images/avatars/") > -1) {
        $scope.UserBasicInfo.ProfileImage = $scope.UserBasicInfo.ProfileImage.replace('/images/avatars/', '');
    }

    for (var i = 0; i < $scope.RewardPkgs.length; i++) {
        if ($scope.RewardPkgs[i].RewardPkgID == $scope.BackData.RewardPKgID) {
            $scope.Packet = $scope.RewardPkgs[i];
        }
    }

    if ($scope.BackData.PledgeAmount == null) {
        $location.path("/project/back/" + $route.current.params.code).replace();
    }

    // Initial backing data record
    $scope.backingData = {};

    $scope.backingData.RewardPKgID = $scope.BackData.RewardPKgID;
    $scope.backingData.PledgeAmount = $scope.BackData.PledgeAmount;
    $scope.backingData.Quantity = $scope.BackData.Quantity;
    $scope.backingData.BackerName = $scope.UserBasicInfo.FullName;
    $scope.backingData.ProjectCode = $scope.BackData.ProjectCode;
    $scope.backingData.Address = $scope.UserBasicInfo.Addres;
    $scope.backingData.Email = $scope.UserBasicInfo.Email;
    $scope.backingData.PhoneNumber = $scope.UserBasicInfo.ContactNumber;
    $scope.backingData.RewardPkgDesc = $scope.Packet.Description;

    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    // Function check string startwith 'http'
    $scope.checkHTTP = function (input) {
        var lowerStr = (input + "").toLowerCase();
        return lowerStr.indexOf('http') === 0;
    }

    $scope.submit = function () {
        //$scope.backingData.BackerName = $scope.UserBasicInfo.FullName;
        //$scope.backingData.ProjectCode = $scope.BackData.ProjectCode;
        //$scope.backingData.Address = $scope.UserBasicInfo.Addres;
        //$scope.backingData.Email = $scope.UserBasicInfo.Email;
        //$scope.backingData.PhoneNumber = $scope.UserBasicInfo.ContactNumber;

        var promisePost = ProjectService.backingProject($scope.backingData);

        promisePost.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Bạn đã ủng hộ dự án thành công!');
                    $window.location.href = "/#/backingdetail/" + result.data.Data;
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

    //$scope.submitBaokim = function () {


    //var baokimUrl = $scope.backingData.ProjectCode + "&PledgeAmount=" + $scope.backingData.PledgeAmount + "&Des=" + $scope.Packet.Description
    //    + "&Email=" + $scope.backingData.Email + "&BackerName=" + $scope.backingData.BackerName + "&RewardId=" + $scope.backingData.RewardPKgID
    //    + "&Quantity=" + $scope.backingData.Quantity + "&Mes=" + $scope.backingData.Description
    //    + "&Address=" + $scope.backingData.Address + "&Phone=" + $scope.backingData.PhoneNumber;

    //$window.location.href = "/baokim?ProjectCode=" + encodeURI(baokimUrl);

    //var promisePost = ProjectService.backingProject($scope.backingData);

    //promisePost.then(
    //    function (result) {
    //        if (result.data.Status === "success") {
    //            //toastr.success('Bạn đã ủng hộ dự án thành công!');
    //            $window.location.href ="/baokim?=";
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
    //}

});