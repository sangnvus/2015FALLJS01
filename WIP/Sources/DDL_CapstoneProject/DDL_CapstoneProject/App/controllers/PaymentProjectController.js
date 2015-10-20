"use strict";

app.controller('PaymentProjectController', function ($scope, $rootScope, $location, toastr, CommmonService, ProjectService, rewardPkgs, usereditinfo) {
    // Get rewardPkgs
    $scope.RewardPkgs = rewardPkgs.data.Data;

    // Get backingdata form backing page
    $scope.BackData = ProjectService.getBack();

    // Get user basic information
    $scope.UserBasicInfo = usereditinfo.data.Data;

    // Initial backing data record
    $scope.backingData = {};

    $scope.paymentFrom = {};

    $scope.backingData.RewardPKgID = $scope.BackData.RewardPKgID;
    $scope.backingData.PledgeAmount = $scope.BackData.PledgeAmount;
    $scope.backingData.Quantity = $scope.BackData.Quantity;

    $scope.submit = function () {
        $scope.backingData.ProjectCode = $scope.BackData.ProjectCode;
        $scope.backingData.Address = $scope.UserBasicInfo.Addres;
        $scope.backingData.Email = $scope.UserBasicInfo.Email;
        $scope.backingData.PhoneNumber = $scope.UserBasicInfo.ContactNumber;

        var promisePost = ProjectService.backingProject($scope.backingData);

        promisePost.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Bạn đã ủng hộ dự án thành công!');
                    $location.path("/project/detail/" + result.data.Data).replace();
                } else {
                    //CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
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