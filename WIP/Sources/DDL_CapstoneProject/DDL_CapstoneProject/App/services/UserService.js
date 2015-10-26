"use strict";

service.service('UserService', function ($http) {

    // Function to create new Account
    this.register = function (newUser) {
        var request = $http({
            method: 'post',
            url: '/api/UserApi/Register',
            data: newUser
        });

        return request;
    }

    // Function to reset password
    this.resetPassword = function (email) {
        var request = $http({
            method: 'post',
            url: '/api/UserApi/ResetPassword?email=' + email,
        });

        return request;
    }

    // Function to get list username filter by value
    this.getUserName = function (val) {
        var request = $http({
            method: 'get',
            url: '/api/UserApi/GetListUserName',
            params: {
                username: val
            }
        });

        return request;
    }

    // Function to check login status
    this.checkLoginStatus = function () {
        var request = $http({
            method: 'get',
            url: '/api/UserApi/CheckLoginStatus',
        });

        return request;
    }

    this.getPublicInformation = function (user) {
        var request = $http({
            method: 'get',
            url: '/api/UserApi/GetPublicInfo',
            params: {
                username: user
            }
        });
        return request;
    }

    this.getProfileInformation = function (user) {
        var request = $http({
            method: 'get',
            url: '/api/UserApi/GetUserInfoEdit'
        });
        return request;
    }

    this.editProfileInformation = function (UserEditInfo, file) {
        var fdata = new FormData();
        var url = "/api/UserApi/EditUserInfo";
        fdata.append('file', file);
        fdata.append('profile', JSON.stringify(UserEditInfo));
        return $http.post(url, fdata, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined },

        })
            .success(function (resp) {
                //debugger;
            })
            .error(function (resp) {
                //debugger;
            });
    }

    this.getEditPassword = function () {
        var request = $http({
            method: 'get',
            url: '/api/UserApi/GetUserPasswordEdit'
        });
        return request;
    }

    this.changepassword = function (newpass) {
        var request = $http({
            method: 'post',
            url: '/api/UserApi/ChangePassword',
            data: newpass
        });
        return request;
    }

    this.getBackedUserInfo = function () {
        var request = $http({
            method: 'get',
            url: '/api/UserApi/GetBackedUserInfo'
        });
        return request;
    }
});