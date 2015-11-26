"use strict";

app.controller('EditProfileController', function ($scope, toastr, usereditinfo, UserService, $filter, $sce, fileReader, CommmonService) {
    //  $scope.UserEditInfo = usereditinfo.data.Data;

    // Todo here.
    $scope.Error = null;

    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    // Function check string startwith 'http'
    $scope.checkHTTP = function (input) {
        var lowerStr = (input + "").toLowerCase();
        return lowerStr.indexOf('http') == 0;
    }

    var promiseGet = UserService.getProfileInformation();

    promiseGet.then(
        function (result) {
            if (result.data.Status === "success") {
                $scope.UserEditInfo = usereditinfo.data.Data;
                if ($scope.UserEditInfo.DateOfBirth != null)
                    $scope.UserEditInfo.DateOfBirth = new Date($filter('date')($scope.UserEditInfo.DateOfBirth, "yyyy-MM-dd"));
            } else {
                CommmonService.checkError(result.data.Type);
                $scope.Error = result.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            }
        },
        function (error) {
            $scope.Error = error.data.Message;
        });


    // Preview image file
    $scope.getFile = function (file) {
        $scope.file = file;
        fileReader.readAsDataUrl($scope.file, $scope)
                      .then(function (result) {
                          $scope.UserEditInfo.ProfileImage = result;
                      });
    };

    // Submit User model to edit user information
    $scope.submit = function () {
        console.log("file: " + $scope.file);
        var promisePost = UserService.editProfileInformation($scope.UserEditInfo, $scope.file);

        promisePost.then(
                function () {
                    if (usereditinfo.data.Status === "success") {
                        toastr.success('Sửa thông tin cá nhân!', 'Thành công!');
                    } else if (usereditinfo.data.Status === "error") {
                        $scope.Error = usereditinfo.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                }
                );
    };
    $scope.units = [
        { 'id': 'male', 'label': 'Nam' },
        { 'id': 'female', 'label': 'Nữ' },
    ]


});