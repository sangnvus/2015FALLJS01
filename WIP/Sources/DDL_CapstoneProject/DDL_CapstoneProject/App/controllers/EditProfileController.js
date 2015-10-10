"use strict";

app.controller('EditProfileController', function ($scope, toastr, usereditinfo, UserService) {
    $scope.UserEditInfo = usereditinfo.data.Data;

    // Todo here.
    $scope.Error = null;
    // Submit User model to edit user information
    $scope.submit = function () {
        var promisePost = UserService.editProfileInformation($scope.UserEditInfo);
        toastr.success('Bạn đã gửi tin nhắn thành công!', 'Thành công!');

        //$scope.error = function () {
        //    fullname.$invalid;
        // };
    };
    $scope.units = [
        { 'id': 'male', 'label': 'Male' },
        { 'id': 'female', 'label': 'Female' },
    ]

    promisePost.then(
            function () {
                if (usereditinfo.data.Status === "success") {
                } else if (usereditinfo.data.Status === "error") {
                    $scope.Error = usereditinfo.data.Message;
                }
            }
            );

});