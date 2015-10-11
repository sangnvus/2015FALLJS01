"use strict";

app.controller('EditProfileController', function ($scope, toastr, usereditinfo, UserService) {
    $scope.UserEditInfo = usereditinfo.data.Data;

    // Todo here.
    $scope.Error = null;
    // Submit User model to edit user information
    $scope.submit = function () {
        var promisePost = UserService.editProfileInformation($scope.UserEditInfo);

        promisePost.then(
                function () {
                    if (usereditinfo.data.Status === "success") {
                        toastr.success('Sửa thông tin cá nhân!', 'Thành công!');
                    } else if (usereditinfo.data.Status === "error") {
                        $scope.Error = usereditinfo.data.Message;
                    }
                }
                );

        //$scope.error = function () {
        //    fullname.$invalid;
        // };
    };
    $scope.units = [
        { 'id': 'male', 'label': 'Male' },
        { 'id': 'female', 'label': 'Female' },
    ]


});